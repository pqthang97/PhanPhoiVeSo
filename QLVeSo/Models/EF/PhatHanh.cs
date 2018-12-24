namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PhatHanh")]
    public partial class PhatHanh
    {
        [Key]
        public int? MaPH { get; set; }

        [Column(Order = 0)]
        [StringLength(20)]
        [Display(Name = "Mã đại lý")]
        public string MaDaiLy { get; set; }

        [Column(Order = 1)]
        [StringLength(20)]
        [Display(Name = "Mã loại vé số")]
        public string MaLoaiVeSo { get; set; }

        [Display(Name = "Số lượng")]
        public int? SoLuong { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Ngày nhận")]
        public DateTime NgayNhan { get; set; }

        [Display(Name = "Số lượng bán")]
        public int? SLBan { get; set; }

        [Display(Name = "Doanh thu đợt phát hành")]
        public decimal? DoanhThuDPH { get; set; }

        [Display(Name = "Hoa hồng")]
        public decimal? HoaHong { get; set; }

        [Display(Name = "Tiền thanh toán")]
        public decimal? TienThanhToan { get; set; }

        public bool? Flag { get; set; }

        public virtual DaiLy DaiLy { get; set; }

        public virtual LoaiVeso LoaiVeso { get; set; }
    }
}
