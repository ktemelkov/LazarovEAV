using Aga.Controls.Tree;
using LazarovEAV.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class SubstanceEditorViewModel : DependencyObject, IDisposable
    {
        public static readonly DependencyProperty EditorModeProperty =
                        DependencyProperty.Register("EditorMode", typeof(EditorMode), typeof(SubstanceEditorViewModel),
                        new FrameworkPropertyMetadata(EditorMode.LIST, (o, e) => { }));

        public EditorMode EditorMode { get { return (EditorMode)GetValue(EditorModeProperty); } set { SetValue(EditorModeProperty, value); } }


        public static readonly DependencyProperty SelectedItemProperty =
                        DependencyProperty.Register("SelectedItem", typeof(SubstanceTreeItemViewModel), typeof(SubstanceEditorViewModel),
                        new FrameworkPropertyMetadata(null, (o, e) => { }));

        public SubstanceTreeItemViewModel SelectedItem { get { return (SubstanceTreeItemViewModel)GetValue(SelectedItemProperty); } set { SetValue(SelectedItemProperty, value); } }



        public static readonly DependencyProperty EditedItemProperty =
                        DependencyProperty.Register("EditedItem", typeof(SubstanceTreeItemViewModel), typeof(SubstanceEditorViewModel),
                        new FrameworkPropertyMetadata(null, (o, e) => { }));

        public SubstanceTreeItemViewModel EditedItem { get { return (SubstanceTreeItemViewModel)GetValue(EditedItemProperty); } set { SetValue(EditedItemProperty, value); } }


        public static readonly DependencyProperty ErrorMessageProperty =
                        DependencyProperty.Register("ErrorMessage", typeof(string), typeof(SubstanceEditorViewModel),
                        new FrameworkPropertyMetadata("", (o, e) => { }));

        public string ErrorMessage { get { return (string)GetValue(ErrorMessageProperty); } set { SetValue(ErrorMessageProperty, value); } }


        public static readonly DependencyProperty TreeContentProviderProperty =
                        DependencyProperty.Register("TreeContentProvider", typeof(ITreeModel), typeof(SubstanceEditorViewModel),
                        new FrameworkPropertyMetadata(null));

        public ITreeModel TreeContentProvider { get { return (ITreeModel)GetValue(TreeContentProviderProperty); } set { SetValue(TreeContentProviderProperty, value); } }

        
        public static readonly DependencyProperty FilterTextProperty =
                        DependencyProperty.Register("FilterText", typeof(string), typeof(SubstanceEditorViewModel),
                        new FrameworkPropertyMetadata("", (o, e) => { ((SubstanceEditorViewModel)o).filterTextChanged((string)e.NewValue); }));

        public string FilterText { get { return (string)GetValue(FilterTextProperty); } set { SetValue(FilterTextProperty, value); } }


        private CommandDelegate editItemCommand;
        public ICommand EditItemCommand { get { return this.editItemCommand; } }

        private CommandDelegate createItemCommand;
        public ICommand CreateItemCommand { get { return this.createItemCommand; } }

        private CommandDelegate createCategoryCommand;
        public ICommand CreateCategoryCommand { get { return this.createCategoryCommand; } }
        
        private CommandDelegate applyCommand;
        public ICommand ApplyCommand { get { return this.applyCommand; } }

        private CommandDelegate applyFolderCommand;
        public ICommand ApplyFolderCommand { get { return this.applyFolderCommand; } }
        
        private CommandDelegate cancelCommand;
        public ICommand CancelCommand { get { return this.cancelCommand; } }

        private CommandDelegate deleteItemCommand;
        public ICommand DeleteItemCommand { get { return this.deleteItemCommand; } }

        private CommandDelegate confirmDeleteItemCommand;
        public ICommand ConfirmDeleteItemCommand { get { return this.confirmDeleteItemCommand; } }

        private EntityManager itemManager;


        /// <summary>
        /// 
        /// </summary>
        public SubstanceEditorViewModel()
        {
            this.editItemCommand = new CommandDelegate(new Action<object>(editItem));
            this.createItemCommand = new CommandDelegate(new Action<object>(createItem));
            this.createCategoryCommand = new CommandDelegate(new Action<object>(createCategory));            
            this.applyCommand = new CommandDelegate(new Action<object>(apply));
            this.applyFolderCommand = new CommandDelegate(new Action<object>(applyFolder));
            this.cancelCommand = new CommandDelegate(new Action<object>(cancel));
            this.deleteItemCommand = new CommandDelegate(new Action<object>(deleteItem));
            this.confirmDeleteItemCommand = new CommandDelegate(new Action<object>(confirmDeleteItem));
            
            if (!DesignerProperties.GetIsInDesignMode(new Mock.MockDependencyObject()))
            {
                this.TreeContentProvider = new SubstanceTreeContentProvider(this.itemManager = new EntityManager());
            }
            else
            {
                this.TreeContentProvider = new SubstanceTreeMockProvider();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.TreeContentProvider = null;
            this.itemManager.Dispose();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void confirmDeleteItem(object obj)
        {
            var parent_id = this.SelectedItem.Parent_Id;

            if (this.SelectedItem.Substance != null)
                this.itemManager.deleteItem(this.SelectedItem.Substance);
            else if (this.SelectedItem.Folder != null)
            {
                var folders = this.getFolderTreeAsFlatList(this.SelectedItem.Folder);

                foreach (var f in folders)
                {
                    this.itemManager.deleteItemsByFilter<SubstanceInfo>(s => s.Folder_Id == f.Id);
                    this.itemManager.deleteItem(f);
                }
            }

            this.itemManager.saveChanges();

            this.TreeContentProvider = new SubstanceTreeContentProvider(this.itemManager, this.FilterText);

            var folder = this.itemManager.getItemById<SubstanceFolder>(parent_id);
            this.SelectedItem = folder != null ? new SubstanceTreeItemViewModel(folder) : null;

            this.EditorMode = EditorMode.LIST;
        }


        private IEnumerable<SubstanceFolder> getFolderTreeAsFlatList(SubstanceFolder parent)
        {
            var folders = (new List<SubstanceFolder>() { parent }).AsEnumerable();

            foreach (var folder in this.itemManager.getItemsByFilter<SubstanceFolder>(x => x.Parent_Id == parent.Id))
            {
                folders = folders.Concat(this.getFolderTreeAsFlatList(folder));
            }

            return folders;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void cancel(object obj)
        {
            backToMainMode();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void apply(object obj)
        {
            if (this.EditedItem.Name == null || this.EditedItem.Name.Length <= 0)
            {
                this.ErrorMessage = "Моля въведете наименование на препарат!";
            }
            else
            {
                if (this.EditorMode == EditorMode.CREATE)
                {
                    this.itemManager.addItem(this.EditedItem.Substance);
                    this.itemManager.saveChanges();

                    this.TreeContentProvider = new SubstanceTreeContentProvider(this.itemManager, this.FilterText);
                    this.SelectedItem = this.EditedItem;
                }
                else if (this.EditorMode == EditorMode.EDIT)
                {
                    this.itemManager.saveChanges();
                    this.EditedItem.CopyTo(this.SelectedItem);
                }

                backToMainMode();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void applyFolder(object obj)
        {
            if (this.EditedItem.Name == null || this.EditedItem.Name.Length <= 0)
            {
                this.ErrorMessage = "Моля въведете наименование на категория!";
            }
            else
            {
                if (this.EditorMode == EditorMode.CREATECAT)
                {
                    this.itemManager.addItem(this.EditedItem.Folder);
                    this.itemManager.saveChanges();

                    this.TreeContentProvider = new SubstanceTreeContentProvider(this.itemManager, this.FilterText);
                    this.SelectedItem = this.EditedItem;
                }
                else if (this.EditorMode == EditorMode.EDITCAT)
                {                    
                    this.itemManager.saveChanges();
                    this.EditedItem.CopyTo(this.SelectedItem);
                }

                backToMainMode();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void backToMainMode()
        {
            this.EditedItem = null;
            this.ErrorMessage = "";
            this.EditorMode = EditorMode.LIST;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void createItem(object obj)
        {
            this.EditorMode = EditorMode.CREATE;

            var item = new SubstanceTreeItemViewModel(new SubstanceInfo());

            if (this.SelectedItem != null)
            {
                if (this.SelectedItem.Folder != null)
                    item.Parent_Id = this.SelectedItem.Folder.Id;
                else
                    item.Parent_Id = this.SelectedItem.Parent_Id;
            }

            this.EditedItem = item;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void createCategory(object obj)
        {
            this.EditorMode = EditorMode.CREATECAT;

            SubstanceTreeItemViewModel sf = new SubstanceTreeItemViewModel(new SubstanceFolder());

            if (this.SelectedItem != null)
            {
                if (this.SelectedItem.Folder != null)
                    sf.Parent_Id = this.SelectedItem.Folder.Id;
                else if (this.SelectedItem.Substance != null)
                    sf.Parent_Id = this.SelectedItem.Parent_Id;
            }

            this.EditedItem = sf;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void deleteItem(object obj)
        {
            if (this.SelectedItem == null)
                return;

            this.EditorMode = EditorMode.CONFIRM_DELETE;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void editItem(object obj)
        {
            if (this.SelectedItem == null || !(this.SelectedItem is SubstanceTreeItemViewModel))
                return;

            if (this.SelectedItem.Substance != null)
            {
                this.EditorMode = EditorMode.EDIT;

                var item = new SubstanceTreeItemViewModel(new SubstanceInfo());
                this.SelectedItem.CopyTo(item);

                this.EditedItem = item;
            }
            else if (this.SelectedItem.Folder != null)
            {
                this.EditorMode = EditorMode.EDITCAT;

                var item = new SubstanceTreeItemViewModel(new SubstanceFolder());
                this.SelectedItem.CopyTo(item);

                this.EditedItem = item;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        private void filterTextChanged(string text)
        {
            this.TreeContentProvider = new SubstanceTreeContentProvider(this.itemManager, text);
        }
    }
}
