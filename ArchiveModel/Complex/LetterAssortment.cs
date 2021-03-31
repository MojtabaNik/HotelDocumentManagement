using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel
{
    [Serializable]
    public class LetterAssortment
    {
        //public int Id { get; set; }
        /// <summary>
        /// like Personal-Administrative
        /// </summary>
        public LetterType LetterType { get; set; }
        /// <summary>
        /// Like Secret-VerySecret-Arcane-WholeArcane
        /// </summary>
        public Security Security { get; set; }
        /// <summary>
        /// Like Imperative-Predicative-Benedictory-
        /// </summary>
        public Nature Nature { get; set; }
        /// <summary>
        /// Like Immediately-Quick-Normal-
        /// </summary>
        public Priority Priority { get; set; }
        /// <summary>
        /// Financial-Cultural-Scientific-Historical-Construction
        /// </summary>
        public ContentType ContentType { get; set; }
        /// <summary>
        /// Current-Stagnant
        /// </summary>
        public Situation Situation { get; set; }
        /// <summary>
        /// like Normal-Legal
        /// </summary>
        public Legalvalidity Legalvalidity { get; set; }
    }
}
