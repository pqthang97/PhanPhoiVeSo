namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhieuThu")]
    public partial class PhieuThu
    {
        [Key]
        [StringLength(20)]
        [Display(Name = "Mã phiếu thu")]
        public string MaPhieuThu { get; set; }

        [StringLength(20)]
        [Display(Name = "Mã đại lý")]
        public string MaDaiLy { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày nộp")]
        public DateTime NgayNop { get; set; }

        [Display(Name = "Số tiền nộp")]
        public decimal? SoTienNop { get; set; }

        public bool? Flag { get; set; }

        public virtual DaiLy DaiLy { get; set; }
    }
}
