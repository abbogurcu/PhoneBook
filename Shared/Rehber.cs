using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rehberv2.Shared
{
    public class Rehber
    {
        [Key]
        public int rehberID { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string isim { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string soyisim { get; set; }


        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string cepTel { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string isTel { get; set; }

        [Required]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string email { get; set; }
    }
}
