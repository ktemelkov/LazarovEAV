using LazarovEAV.Model;
using Newtonsoft.Json;


namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class EffectiveSubstanceInfoViewModel
    {
        private EffectiveSubstanceInfo model;
        public EffectiveSubstanceInfo Model { get { return this.model; } }

        public string Name { get { return this.model.Name; } }
        public string Description { get { return this.model.Description; } }
        public SubstanceType Type { get { return this.model.Type; } }
        public string Quantity { get { return this.model.Quantity; } }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public EffectiveSubstanceInfoViewModel(EffectiveSubstanceInfo m)
        {
            this.model = m;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="quantity"></param>
        public EffectiveSubstanceInfoViewModel(string name, string descr, int type, string quantity)
        {
            this.model = new EffectiveSubstanceInfo() { Name = name, Type = (SubstanceType)type, Description = descr, Quantity = quantity };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public EffectiveSubstanceInfoViewModel(string json)
        {
            this.JSON = json;
        }


        /// <summary>
        /// 
        /// </summary>
        public string JSON
        {
            get 
            {
                return JsonConvert.SerializeObject(this.model);
            }

            set 
            {
                this.model = JsonConvert.DeserializeObject<EffectiveSubstanceInfo>(value);
            }
        }
    }
}
