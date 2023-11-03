using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlingProduct {
    class ClassOption {
        /// <summary>
              /// 옵션 아이디
        /// </summary>
        public string opt_id { get; set; }
        /// <summary>
              /// 옵션가격
        /// </summary>
        public string opt_price { get; set; }
        /// <summary>
              /// 옵션1 번호
        /// </summary>
        public string opt1_id { get; set; }
        /// <summary>
              /// 옵션1명
        /// </summary>
        public string opt1_name { get; set; }
        /// <summary>
               /// 옵션1이미지
        /// </summary>
        public string opt1_image { get; set; }
        /// <summary>
               /// 옵션2번호
        /// </summary>
        public string opt2_id { get; set; }
        /// <summary>
               /// 옵션2명
        /// </summary>
        public string opt2_name { get; set; }
        /// <summary>
              /// 옵션2이미지
        /// </summary>
        public string opt2_image { get; set; }
        /// <summary>
              ///  옵션3번호
        /// </summary>
        public string opt3_id { get; set; }
        /// <summary>
              /// 옵션3명
        /// </summary>
        public string opt3_name { get; set; }
        /// <summary>
              /// 옵션3이미지
        /// </summary>
        public string opt3_image { get; set; }
    }
}

class ClassOptionFlat {
    public int opt_level { get; set; }
    /// <summary>
    /// 옵션1 번호
    /// </summary>
    public string opt_id { get; set; }
  /// <summary>
    /// 옵션1명
  /// </summary>
    public string opt_name { get; set; }
  /// <summary>
    /// 옵션1이미지
  /// </summary>
    public string opt_image { get; set; }
}
