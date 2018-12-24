namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuChi")]
    public partial class PhieuChi
    {
        [Key]
        [StringLength(20)]
        [Display(Name = "Mã phiếu chi")]
        public string MaPhieuChi { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày")]
        public DateTime Ngay { get; set; }

        [StringLength(200)]
        [Display(Name = "Nội dung")]
        public string NoiDung { get; set; }

        public decimal? SoTien { get; set; }
    }
}
