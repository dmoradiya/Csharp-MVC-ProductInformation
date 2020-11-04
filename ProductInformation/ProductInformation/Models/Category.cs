using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductInformation.Models
{
    [Table("Category")]
    public class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }
        [Key]
        [Column(TypeName = "int(10)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Column(TypeName = "varchar(30)")]
        [Required]
        public string Name { get; set; }

        [InverseProperty(nameof(Models.Product.Category))]
        public virtual ICollection<Product> Products { get; set; }

    }
}
