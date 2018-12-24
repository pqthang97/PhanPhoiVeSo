namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KetQuaSoXo")]
    public partial class KetQuaSoXo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [StringLength(20)]
        [Display(Name = "Mã loại vé số")]
        public string MaLoaiVeSo { get; set; }

        [StringLength(20)]
        [Display(Name = "Mã giải")]
        public string MaGiai { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày Xổ")]
        public DateTime NgaySo { get; set; }

        [StringLength(10)]
        [Display(Name = "Số trúng")]
        public string SoTrung { get; set; }

        public bool? Flag { get; set; }

        public virtual Giai Giai { get; set; }

        public virtual LoaiVeso LoaiVeso { get; set; }
    }
}
