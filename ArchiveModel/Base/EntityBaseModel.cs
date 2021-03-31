using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityModel
{
    public class EntityBaseModel
    {
        public EntityBaseModel()
        {
            TimeStamp = DateTime.Now;
            Active = true;
        }
        [Key,Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsActive()
        {
            return Active;
        }
    }
}
