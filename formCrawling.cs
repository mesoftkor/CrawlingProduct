using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.IO;
using System.Net.NetworkInformation;
using CefSharp.DevTools.DOM;
using CefSharp.Internals;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace CrawlingProduct {
	public partial class formCrawling : Form {

		string strProductUrl = "https://item.taobao.com/item.htm?abbucket=15&id=";
		string strUnusualCheckImage = "https://img.alicdn.com/imgextra/i2/O1CN010VLpQY1VWKHBQuBUQ_!!6000000002660-2-tps-222-222.png";
		string strProductNum;//, strUserId, strPasswd;
		bool isLogin = false;
		int intDataRowNum = -1;
		string strUrl = "";

		DataTable dtProductList;
		public formCrawling() {
			InitializeComponent();
			InitializeCefSharp();
			//GetModifiedMac();
			//ID, Pass 임시로 가져오기
			if (File.Exists(Application.StartupPath + "\\login.txt")) {
				foreach (string strReadLine in File.ReadLines(Application.StartupPath + "\\login.txt")) {
					string[] strLogin = strReadLine.Trim().Split(' ');
					if (strLogin.Length == 2) {
						txtId.Text = strLogin[0];
						txtPass.Text = strLogin[1];
					}
				}
				gridProductList.DataSource = dtProductList;
			}

			//시작폴더에 product_list.txt 파일이 있으면 자동으로 가져옴.
			InitDataTable();
			if (File.Exists(Application.StartupPath + "\\product_list.txt")) {
				foreach (string strReadLine in File.ReadLines(Application.StartupPath + "\\product_list.txt")) {
					AddRow(strReadLine.Trim());
				}
				gridProductList.DataSource = dtProductList;
			}

		}
		/// <summary>
		/// 상품리스트와 상태를 저장할 DataTable 초기화
		/// </summary>
		private void InitDataTable() {
			dtProductList = new DataTable();
			dtProductList.Columns.Add("num");
			dtProductList.Columns.Add("product_num");
			dtProductList.Columns.Add("product_type");
			dtProductList.Columns.Add("complete");
		}

		private void InitializeCefSharp() {
			//쿠키 데이터 사용하는 방법
			CefSettings settings = new CefSettings();
			settings.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CEF";
			if (!Cef.IsInitialized)
				Cef.Initialize(settings);

			//웹 사이트 이동
			//_chrome = new ChromiumWebBrowser("https://login.taobao.com/member/login.jhtml");
			//_chrome.LoadUrl("https://login.taobao.com/member/login.jhtml");
			//한국어 설정
			//_chrome.BrowserSettings.AcceptLanguageList = "ko-KR";
			//Main Form에 CefSharp컨트롤 추가
			//this.Controls.Add(_chrome);
			//Main Form 전체 영역에 붙이기
			//_chrome.Dock = DockStyle.Fill;
			//페이지 로딩 완료 이벤트
			_chrome.LoadingStateChanged += OnLoadingStateChanged;
			_chrome.AddressChanged += Browser_AddressChanged;
		}

		private void initBrowser() {
			_chrome = new CefSharp.WinForms.ChromiumWebBrowser();
			tabBrowser.Controls.Add(this._chrome);
			tabBrowser.Location = new System.Drawing.Point(4, 22);
			tabBrowser.Name = "tabBrowser";
			tabBrowser.Padding = new System.Windows.Forms.Padding(3);
			tabBrowser.Size = new System.Drawing.Size(908, 466);
			tabBrowser.TabIndex = 0;
			tabBrowser.Text = "Browser";
			tabBrowser.UseVisualStyleBackColor = true;
			//_chrome.LoadingStateChanged += OnLoadingStateChanged;
			//_chrome.AddressChanged += Browser_AddressChanged;
		}

		#region Crawing Product
		private async void Login() {
			//使用其他账号登录
			_chrome.LoadUrl("https://login.taobao.com/member/login.jhtml");
			if (await CheckLogin()) {
				return;
			}
			PrintStatusConsole("로그인 처리중....");
			string id = txtId.Text.Trim();
			string pwd = txtPass.Text.Trim();
			_chrome.ExecuteScriptAsync("document.getElementById('fm-login-id').focus();");
			await Task.Delay(500);
			_chrome.ExecuteScriptAsync("document.getElementById('fm-login-id').value=" + '\'' + id + '\'');
			await Task.Delay(500);
			_chrome.ExecuteScriptAsync("document.getElementById('fm-login-password').focus();");
			await Task.Delay(500);
			_chrome.ExecuteScriptAsync("document.getElementById('fm-login-password').value=" + '\'' + pwd + '\'');
			await Task.Delay(500);
			ClickButtonUsingXPath(@"//*[@id='login-form']/div[4]/button");

			await Task.Delay(3000);

			//_chrome.ExecuteScriptAsync("document.getElementById('login-form').submit();");
			//ti.Stop();
			isLogin = true;
		}

		/// <summary>
		/// 로그인 상태인지 체크
		/// </summary>
		/// <returns>true:이미로그인, false: 로그인처리 필요</returns>
		private async Task<bool> CheckLogin() {
			await Task.Delay(4000);
			JavascriptResponse response = await _chrome.EvaluateScriptAsync("document.documentElement.outerHTML");
			string htmlContent = "";
			if (response.Success && response.Result != null) {
				htmlContent = response.Result.ToString();
				if (htmlContent.Contains("has-login-user")) {
					return true;
				}
			}
			return false;
		}
		private void ClickButtonUsingXPath(string xpath) {
			string script = string.Format(@"
		var iterator = document.evaluate('{0}', document, null, XPathResult.UNORDERED_NODE_ITERATOR_TYPE, null );
		var button = iterator.iterateNext();
		if (button) {{
			button.click();
		}}
	", xpath.Replace("'", "\\'")); // Escape any single quotes in the XPath for JavaScript
			_chrome.EvaluateScriptAsync(script).ContinueWith(t =>
			{
				if (t.Result.Success) {
					Console.WriteLine($"Button with XPath '{xpath}' was clicked.");
				}
				else {
					Console.WriteLine($"Error clicking button with XPath '{xpath}'.");
				}
			});
		}



		private bool CheckValiable() {
			if (txtId.Text.Trim() == "") {
				MessageBox.Show("ID가 입력되지 않았습니다.");
				return false;
			}
			if (txtPass.Text.Trim() == "") {
				MessageBox.Show("패스워드가 입력되지 않았습니다.");
				return false;
			}
			if (dtProductList.Rows.Count == 0) {
				MessageBox.Show("상품리스트가 설정되지 않았습니다.");
				return false;
			}
			return true;
		}

		private async void CrawingProductStart() {
			//필수항목 체크
			CheckValiable();
			//로그인처리
			Login();
			//자료수집 시작
			await Task.Run(() =>
			{
				NextProductCall();
				System.Threading.Thread.Sleep(2000);
			});

		}

		private void NextProductCall() {
			//현재 확인 라인이 Completed가 아닌경우 다시 재시도
			if (intDataRowNum < 0 || dtProductList.Rows[intDataRowNum]["complete"].ToString() == "Completed" || dtProductList.Rows[intDataRowNum]["complete"].ToString() == "Not Completed")
				intDataRowNum++;
			if (dtProductList.Rows.Count - 1 == intDataRowNum) {
				PrintStatusConsole("종료");
				return;//종료!!!!
			}
			strProductNum = dtProductList.Rows[intDataRowNum]["product_num"].ToString();
			_chrome.LoadUrl(strProductUrl + strProductNum);
		}

		private async void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args) {
			Console.WriteLine(string.Format("[{0}]{1}", strProductNum, strUrl));
			if (!args.IsLoading && strUrl.Contains(strProductNum)) {
				//블럭 우회 체크
				//if ()
				//    GetModifiedMac();
				//initBrowser();
				//    Login();
				//    return;
				//    }
				//}

				//현재 html에 Sorry, we have detected unusual traffic from your network. 가 뜨는지 확인
				//https://img.alicdn.com/imgextra/i2/O1CN010VLpQY1VWKHBQuBUQ_!!6000000002660-2-tps-222-222.png

				//일단 tmall이 처리되지 않으므로 주소가 tmall이면 패스하도록 세팅
				if(strUrl.Contains("detail.tmall.com")) {
					dtProductList.Rows[intDataRowNum]["product_type"] = "Unknown";
					dtProductList.Rows[intDataRowNum]["complete"] = "Not Completed";
					//intDataRowNum++;
					await Task.Delay(2000);
					NextProductCall();
					return;
				}

				//1. html을 가져오기 전에 아래로 스크롤을 하지 않으면 상세 이미지가 로드되지 않으므로 scroll후 html 가져오기
				await Task.Delay(3000);
				PrintStatusConsole("스크롤링 시작 ");
				//await _chrome.EvaluateScriptAsync("window.scrollTo(0, document.body.scrollHeight-200);");
				var returnValue = await _chrome.EvaluateScriptAsPromiseAsync(@"
let scrollAmount = 800;
let interval = setInterval(() => {
	let scrolled = window.scrollY;
	let maxScroll = document.documentElement.scrollHeight - window.innerHeight;

	if (scrolled < maxScroll) {
		window.scrollBy(0, scrollAmount);
	} else {
		clearInterval(interval); // 맨 밑에 도착하면 setInterval을 중지
	}
}, 300);
return true;
");
				PrintStatusConsole("스크롤링 끝 ");
				// 2. 로딩 대기: 여기서는 13초를 기다립니다. 위의 스크롤링이 종료되는 시점을 일단은 알 수 없으므로 대기후 html 값을 가져옴
				await Task.Delay(13000);

				// JavaScript를 실행하여 페이지의 innerHTML을 가져옵니다.
				JavascriptResponse response = await _chrome.EvaluateScriptAsync("document.documentElement.outerHTML");

				/*//기존 HTML 전체를 가져와서 폴더에 저장하는 형태
				 * 

								//디렉토리가 없는지 체크해서 생성
								if (!Directory.Exists(Application.StartupPath + "\\taobao")) {
									Directory.CreateDirectory(Application.StartupPath + "\\taobao");
								}
								if (response.Success && response.Result != null) {
									System.IO.File.WriteAllText(string.Format(@"{0}\taobao\{1}.txt", Application.StartupPath, strProductNum), response.Result.ToString());
								}
								
				*/
				if (response.Success && response.Result != null) {
					ParseHtmlWithXpathToDataTable(response.Result.ToString());
					dtProductList.Rows[intDataRowNum]["complete"] = "Completed";
				}
				else {
					dtProductList.Rows[intDataRowNum]["complete"] = "Not Completed";
				}

				PrintStatusConsole(strProductNum + "상품 저장 완료");
				NextProductCall();
			}
			else if (!args.IsLoading && strUrl.Contains("https://login.taobao.com")) {
				Login();
				return;
			}
		}

		private void Browser_AddressChanged(object sender, AddressChangedEventArgs e) {
			this.Invoke(new Action(delegate () {
				Console.WriteLine("주소변경됨 : " + e.Address);
				txtUrl.Text = e.Address;
			}));

			strUrl = e.Address;
			//Sorry, we have detected unusual traffic from your network.
			//https://item.taobao.com//item.htm/_____tmd_____/punish?x5secdata=xcnmixV99plQOAPoH5d%2bD28UE5hScl3OergCmiBg61%2bT8iiPMKCS8OcJytUhUpPyB3oPqrH8%2f8cx%2fH44Sfv0aUEdF4TuM5bmAuDMOO%2fOK3ZFjXlmo9BM603Oo4AW0c72M8XxvvwSnfLcWkGNU3tAOU0UHlxweVd%2fWyn4%2f7QpEC8efiibMQQbzFuqGxO7xGqMxQv8bU5yuz%2fzlJsc8iueL5EQ%3d%3d__bx__item.taobao.com%2fitem.htm&x5step=1

			if (e.Address == "https://world.taobao.com/" || e.Address.Contains("https://i.taobao.com/my_taobao.htm?")) { // || e.Address.Contains("https://detail.tmall.com/item.htm")) {
				NextProductCall();
				//var cookie = Cef.GetGlobalCookieManager();
				//cookie.Visi
				//intStep++;
				//ti.Start();
			}
		}
		#endregion

		#region PasingHtml
		private void ParseHtmlWithXpathToDataTable(string htmlContent) {
			var doc = new HtmlAgilityPack.HtmlDocument();
			doc.LoadHtml(htmlContent);

			//상품명으로 타입 체크
			// //*[@id='root']/div/div[2]/div[1]/a/div[1]/div[1]/div
			if (doc.DocumentNode.SelectSingleNode("//*[@id='J_Title']/h3") != null) {
				dtProductList.Rows[intDataRowNum]["product_type"] = "taobao";
				ParseHtmlToJsonTypeTaobao(doc, htmlContent);
			}
			else {
				dtProductList.Rows[intDataRowNum]["product_type"] = "Unknown";
				dtProductList.Rows[intDataRowNum]["complete"] = "Not Completed";
			}
			//else if (doc.DocumentNode.SelectSingleNode("//*[@id='root']/div/div[2]/div[2]/div[1]/div/div[2]/div[1]/h1") != null) {
			//    dtProductList.Rows[intDataRowNum]["product_type"] = "tmall";
			//    ParseHtmlToJsonTypeTmall(doc);
			//}

			// /html/body/div[6]/div/div[3]/div[1]/div[1]/div[1]/div/div[2]/div/div/div[1]/h3
			//this.Invoke(new Action(delegate () {
			//gridControl1.DataSource = dt;
			//    MessageBox.Show("파싱완료");
			//}));


		}

		/// <summary>
		/// Taobao HTML Pasing Type B
		/// 테스트 상품 : 712764371053
		/// </summary>
		/// <param name="doc"></param>
		private void ParseHtmlToDataTableTypeTaobao(HtmlAgilityPack.HtmlDocument doc) {
			string strData = "";
			HtmlAgilityPack.HtmlNodeCollection nodes;
			HtmlAgilityPack.HtmlNode selNode;
			DataRow dr;
			AddDataRow("Product Type", "Taobao");
			//샵네임
			//selNode = doc.DocumentNode.SelectSingleNode("/html/body/div[6]/div/div[2]/div/div/div/div/div[1]/div/div[1]/a/img");
			//if (selNode != null) {
			//    AddDataRow("샵네임", selNode.Attributes["src"].Value);
			//}

			//상품명
			//selNode = doc.DocumentNode.SelectSingleNode("/html/body/div[6]/div/div[3]/div[1]/div[1]/div[1]/div/div[2]/div/div/div[1]/h3");
			selNode = doc.DocumentNode.SelectSingleNode("//*[@id='J_Title']/h3");
			if (selNode != null) {
				AddDataRow("상품명", selNode.InnerText.Trim());
			}

			//옵션1차
			////*[@id="J_isku"]/div/dl[1]/dd/ul/li
			nodes = doc.DocumentNode.SelectNodes("//*[@id=\"J_isku\"]/div/dl[1]/dd/ul/li");
			if (nodes != null) {
				for (int i = 0; i < nodes.Count; i++) {
					//var selectNode = doc.DocumentNode.SelectSingleNode(string.Format("/html/body/div[6]/div/div[3]/div[2]/div[1]/div[1]/div/div[2]/div/div/div[7]/div/dl[1]/dd/ul/li[{0}]/a/span", i + 1));
					var selectNode = doc.DocumentNode.SelectSingleNode(string.Format("//*[@id=\"J_isku\"]/div/dl[1]/dd/ul/li[{0}]/a/span", i + 1));
					if (selectNode != null) {
						AddDataRow("옵션1[" + i + "]", selectNode.InnerText);
					}
				}
			}

			//옵션2차
			//nodes = doc.DocumentNode.SelectNodes("//*[@id='root']/div/div[2]/div[2]/div[1]/div/div[2]/div[5]/div/div/div[1]/div[2]/div/div");
			//if (nodes != null) {
			//    for (int i = 0; i < nodes.Count; i++) {
			//        var selectNode = doc.DocumentNode.SelectSingleNode(string.Format("//*[@id='root']/div/div[2]/div[2]/div[1]/div/div[2]/div[5]/div/div/div[1]/div[2]/div/div[{0}]/div/span", i + 1));
			//        if (selectNode != null) {
			//            AddDataRow("옵션2[" + (i + 1) + "]", selectNode.InnerText);
			//        }
			//    }
			//}
			//
			//Item상세정보
			nodes = doc.DocumentNode.SelectNodes("//*[@id='attributes']/ul/li");
			if (nodes != null) {
				for (int i = 0; i < nodes.Count; i++) {
					var selectNode = doc.DocumentNode.SelectSingleNode(string.Format("//*[@id='attributes']/ul/li[{0}]", i + 1));
					if (selectNode != null) {
						AddDataRow("Item상세정보[" + (i + 1) + "]", selectNode.InnerText.Replace("&nbsp;", ""));
					}
				}
			}

			//상세이미지정보
			////*[@id="root"]/div/div[2]/div[2]/div[3]/div[2]/div[1]/div/div[2]/div/div[1]/img
			// /html/body/div[6]/div/div[3]/div[4]/div[1]/div[1]/div[1]/div[4]/div/div/div/img[1]
			// /html/body/div[6]/div/div[3]/div[4]/div[1]/div[1]/div[1]/div[4]/div/div/div/img[2]
			nodes = doc.DocumentNode.SelectNodes("//*[@id='J_DivItemDesc']/p[3]/img");
			if (nodes != null) {
				for (int i = 0; i < nodes.Count - 1; i++) {
					var selectNode = doc.DocumentNode.SelectSingleNode(string.Format("//*[@id='J_DivItemDesc']/p[3]/img[{0}]", i + 1));
					if (selectNode != null) {
						AddDataRow("상세이미지[" + (i + 1) + "]", selectNode.Attributes["src"].Value);
					}
				}
			}
		}

		private void ParseHtmlToJsonTypeTaobao(HtmlAgilityPack.HtmlDocument doc, string strHtml) {
			string strData = "";
			HtmlAgilityPack.HtmlNodeCollection nodes;
			HtmlAgilityPack.HtmlNode selNode;
			AddDataRow("Product Type", "Taobao");

			JObject Jdata = new JObject();


			//샵네임
			//selNode = doc.DocumentNode.SelectSingleNode("/html/body/div[6]/div/div[2]/div/div/div/div/div[1]/div/div[1]/a/img");
			//if (selNode != null) {
			//    AddDataRow("샵네임", selNode.Attributes["src"].Value);
			//}

			//상품명
			//selNode = doc.DocumentNode.SelectSingleNode("/html/body/div[6]/div/div[3]/div[1]/div[1]/div[1]/div/div[2]/div/div/div[1]/h3");
			selNode = doc.DocumentNode.SelectSingleNode("//*[@id='J_Title']/h3");
			if (selNode != null) {
				AddDataRow("상품명", selNode.InnerText.Trim());
				Jdata.Add("title", selNode.InnerText.Trim());
			}

			//상품 메인 이미지
			string strMainImage = FindauctionImages(strHtml);
			
			Jdata.Add("main_img", JArray.Parse(strMainImage));

			var optList = new List<ClassOption> { }; //json 출력을 위한 객체
			var optFlat = new List<ClassOptionFlat> { };//옵션전체를 저장하기 위한 flat 객체
			/*  alt 값은 사용하지 않음
			propertyMemoMap: {
				"1627207:9482005796":"玫瑰金【勺+筷子+盒+袋】",
				"1627207:9482005795":"金色【勺+筷子+叉+盒】",
			}

			//가격정보를 가져오는데 사용함.
			skuMap     : {
				";1627207:9481880357;":{"price":"8.00","stock":"2","skuId":"4605431747564","oversold":false},
				";1627207:9482005794;":{"price":"11.50","stock":"2","skuId":"46054397952","oversold":false},
				";1627207:9482005795;":{"price":"10.50","stock":"2","skuId":"4605439795220","oversold":false}
			}
			 */
			
			GetOption(1, ref doc, ref optFlat);//옵션1차   //*[@id="J_isku"]/div/dl[1]/dd/ul/li
			GetOption(2, ref doc, ref optFlat);//옵션2차   //*[@id="J_isku"]/div/dl[2]/dd/ul/li		
			GetOption(3, ref doc, ref optFlat);//옵션3차   //*[@id="J_isku"]/div/dl[3]/dd/ul/li

			

			ClassOption cate;
			string strSkuMap = FindSkuMap(strHtml);
			JToken jt = Newtonsoft.Json.Linq.JToken.Parse(strSkuMap);
			string[] strOptId;

            foreach (JProperty j in jt) {
				cate = new ClassOption();
				strOptId = j.Name.Split(new char[] { '|' },StringSplitOptions.RemoveEmptyEntries);
				if (strOptId == null || strOptId.Length == 0)
					break;
				if (strOptId.Length >= 1) {
					var optItem = optFlat.FirstOrDefault(item => item.opt_level == 1 && item.opt_id == strOptId[0]);
					if (optItem != null) {
						cate.opt1_id = optItem.opt_id;
						cate.opt1_image = optItem.opt_image;
						cate.opt1_name = optItem.opt_name;
					}
				}
				if (strOptId.Length >= 2) {
					var optItem = optFlat.FirstOrDefault(item => item.opt_level == 2 && item.opt_id == strOptId[1]);
					if (optItem != null) {
						cate.opt2_id = optItem.opt_id;
						cate.opt2_image = optItem.opt_image;
						cate.opt2_name = optItem.opt_name;
					}
				}
				if (strOptId.Length == 3) {
					var optItem = optFlat.FirstOrDefault(item => item.opt_level == 3 && item.opt_id == strOptId[2]);
					if (optItem != null) {
						cate.opt3_id = optItem.opt_id;
						cate.opt3_image = optItem.opt_image;
						cate.opt3_name = optItem.opt_name;
					}
				}
				cate.opt_id = j.Name;//";1627207:9481880357;"
				cate.opt_price = j.Value["price"].Value<string>();
				optList.Add(cate);
			}
			//Jdata.Add("item_option", JsonConvert.SerializeObject(optList));
			Jdata.Add("item_option", JArray.FromObject(optList));




			//if (strProperty != null) {//propertyMemoMap을 가져오면 검색 시작

			//	cate = new ClassOption();
			//	JObject jsonProperty = JObject.Parse(strProperty);
			//	if (strSkuMap != "") {
			//		// 일치하는 부분을 출력합니다.
			//		JObject json = JObject.Parse(strSkuMap);
			//		JToken jt = Newtonsoft.Json.Linq.JToken.Parse(strProperty);
			//		//JToken jt = json.

			//		foreach (JProperty j in jt) {
			//			cate.op_id = j.Name.Replace(";", "");//";1627207:9481880357;"
			//			cate.op_price = j["price"].ToString();
			//			cate.op_name = jsonProperty[cate.op_id].ToString();

			//		}
			//		optList.Add(cate);
			//	}
			//}
			//else {
			//	Console.WriteLine("일치하는 항목을 찾을 수 없습니다.");
			//}
			//Jdata.Add(optList);

			//옵션2차
			//nodes = doc.DocumentNode.SelectNodes("//*[@id='root']/div/div[2]/div[2]/div[1]/div/div[2]/div[5]/div/div/div[1]/div[2]/div/div");
			//if (nodes != null) {
			//    for (int i = 0; i < nodes.Count; i++) {
			//        var selectNode = doc.DocumentNode.SelectSingleNode(string.Format("//*[@id='root']/div/div[2]/div[2]/div[1]/div/div[2]/div[5]/div/div/div[1]/div[2]/div/div[{0}]/div/span", i + 1));
			//        if (selectNode != null) {
			//            AddDataRow("옵션2[" + (i + 1) + "]", selectNode.InnerText);
			//        }
			//    }
			//}
			//
			//Item상세정보
			List<string> listItemDetail = new List<string>();
			nodes = doc.DocumentNode.SelectNodes("//*[@id='attributes']/ul/li");
			if (nodes != null) {
				for (int i = 0; i < nodes.Count; i++) {
					var selectNode = doc.DocumentNode.SelectSingleNode(string.Format("//*[@id='attributes']/ul/li[{0}]", i + 1));
					if (selectNode != null) {
						//AddDataRow("Item상세정보[" + (i + 1) + "]", selectNode.InnerText.Replace("&nbsp;", ""));
						//Jdata.Add("Item_detail[" + (i + 1) + "]", selectNode.InnerText.Replace("&nbsp;", ""));
						listItemDetail.Add(selectNode.InnerText.Replace("&nbsp;", ""));
					}
				}
			}
			Jdata.Add("item_detail", JArray.FromObject(listItemDetail));

			//상세이미지정보 - 1타입
			// //*[@id="J_DivItemDesc"]/div/div[3]/img
			// //*[@id="J_DivItemDesc"]/div/div[4]/img
			nodes = doc.DocumentNode.SelectNodes("//*[@id='J_DivItemDesc']/div/div");
			List<string> listDetailImage = new List<string>();
			if (nodes != null) {
				for (int i = 3; i <= nodes.Count; i++) {
					var selectNode = doc.DocumentNode.SelectSingleNode(string.Format("//*[@id='J_DivItemDesc']/div/div[{0}]/img", i));
					if (selectNode != null) {
						//AddDataRow("상세이미지[" + (i - 2) + "]", selectNode.Attributes["src"].Value);
						//Jdata.Add("Item_detail_image[" + (i - 2) + "]", selectNode.Attributes["src"].Value);
						listDetailImage.Add(selectNode.Attributes["src"].Value);
					}
				}
            }
            else {
				//상세이미지정보-2타입
				//*[@id="J_DivItemDesc"]/p[1]/img[2]
				////*[@id="root"]/div/div[2]/div[2]/div[3]/div[2]/div[1]/div/div[2]/div/div[1]/img
				// /html/body/div[6]/div/div[3]/div[4]/div[1]/div[1]/div[1]/div[4]/div/div/div/img[1]
				// /html/body/div[6]/div/div[3]/div[4]/div[1]/div[1]/div[1]/div[4]/div/div/div/img[2]
				//int intImageCount = 1;
				nodes = doc.DocumentNode.SelectNodes("//*[@id='J_DivItemDesc']/p");
				if (nodes != null) {
					for (int i = 0; i < nodes.Count; i++) {
						var ImagesNodes = doc.DocumentNode.SelectNodes(string.Format("//*[@id='J_DivItemDesc']/p[{0}]/img", i+1));
						if (ImagesNodes == null) continue;
						for (int j = 0; j < ImagesNodes.Count; j++) {
							var selectNode = doc.DocumentNode.SelectSingleNode(string.Format(string.Format("//*[@id='J_DivItemDesc']/p[{0}]/img[{1}]", i + 1, j + 1)));
							if (selectNode != null) {
								//AddDataRow("상세이미지[" + intImageCount + "]", selectNode.Attributes["src"].Value);
								//Jdata.Add("Item_detail_image[" + intImageCount + "]", selectNode.Attributes["src"].Value);
								listDetailImage.Add(selectNode.Attributes["src"].Value);
								//intImageCount++;
							}
						}
					}
				}
			}

			Jdata.Add("detail_img", JArray.FromObject(listDetailImage));
			//*[@id="J_DivItemDesc"]/p[1]/img[1]

			string strJson = JsonConvert.SerializeObject(Jdata);
			System.IO.File.WriteAllText((strProductNum==null?"TEST":strProductNum)+".json", strJson);
		}

		/// <summary>
		/// 각 카테고리별 옵션을 가져온다.
		/// </summary>
		/// <param name="intOptionLevel"></param>
		/// <param name="doc"></param>
		/// <param name="optFlat"></param>
		private void GetOption(int intOptionLevel, ref HtmlAgilityPack.HtmlDocument doc, ref List<ClassOptionFlat> optFlat) {
			string strXpathOption1 = "//*[@id=\"J_isku\"]/div/dl["+ intOptionLevel + "]/dd/ul/li";
			var nodes = doc.DocumentNode.SelectNodes(strXpathOption1);//node에서 id와 같은 node 찾아서 backgroup:url을 가져와서 op_image에 넣기
			if (nodes != null) {
				for (int i = 0; i < nodes.Count; i++) {
					ClassOptionFlat opt = new ClassOptionFlat();
					opt.opt_level = intOptionLevel;
					opt.opt_id = nodes[i].Attributes["data-value"].Value;//option id
					opt.opt_name = doc.DocumentNode.SelectSingleNode(string.Format("{0}[{1}]/a/span", strXpathOption1, i + 1)).InnerText;//option name
					var selectNode = doc.DocumentNode.SelectSingleNode(string.Format("{0}[{1}]/a", strXpathOption1, i + 1));
					if (selectNode != null && selectNode.Attributes.Contains("style")) {
						//<a href="javascript:;" style="background:url(//gd2.alicdn.com/imgextra/i3/3782733382/O1CN01Mxzkgq1ar074FBBTr_!!3782733382.jpg_30x30.jpg) center no-repeat;" data-spm-anchor-id="2013.1.iteminfo.5">
						opt.opt_image = selectNode.Attributes["style"].Value.Replace(@"background:url(//", "").Replace(") center no-repeat;", "");
					}
					else {
						opt.opt_image = "";
					}
					optFlat.Add(opt);
				}
			}
		}

		private string FindPattern(string strText, string strPattern) {
			// 정규식을 사용하여 일치하는 부분을 찾습니다.
			//replace 보다 속도가 늦어 replace로 변경
			Match match = Regex.Match(strText, strPattern);
			return (match.Success) ? match.Groups[1].Value : "";
		}

		private string FindSkuMap(string strText) {
			int intStart, intEnd;
			intStart = strText.IndexOf("skuMap     : {", 0);
			intEnd = strText.IndexOf("}}", intStart);
			return strText.Substring(intStart + 13, intEnd - intStart - 11).Replace(";", "|");
		}
		private string FindpropertyMemoMap(string strText) {
			int intStart, intEnd;
			intStart = strText.IndexOf("propertyMemoMap: {", 0);
			intEnd = strText.IndexOf("}", intStart);
			//return strText.Substring(intStart, intEnd - intStart + 1).Replace("propertyMemoMap", "\"propertyMemoMap\"");
			return strText.Substring(intStart + 17, intEnd - intStart - 16);
		}

		private string FindauctionImages(string strText) {
			int intStart, intEnd;
			intStart = strText.IndexOf("auctionImages    : ", 0);
			intEnd = strText.IndexOf("]", intStart);
			return strText.Substring(intStart + 19, intEnd - intStart - 18);
		}

		private void ParseHtmlToJsonTypeTmall(HtmlAgilityPack.HtmlDocument doc) {
			string strData = "";
			HtmlAgilityPack.HtmlNodeCollection nodes;
			HtmlAgilityPack.HtmlNode selNode;

			AddDataRow("Product Type", "Tmall");
			JObject Jdata = new JObject();
			//샵네임
			//selNode = doc.DocumentNode.SelectSingleNode("//*[@id='root']/div/div[2]/div[1]/a/div[1]/div[1]/div");
			//if (selNode != null) {
			//    AddDataRow("샵네임", selNode.InnerText);
			//}

			//상품명
			selNode = doc.DocumentNode.SelectSingleNode("//*[@id='root']/div/div[2]/div[2]/div[1]/div/div[2]/div[1]/h1");
			if (selNode != null) {
				Jdata.Add("product_name", selNode.InnerText);
			}
			//옵션1차
			nodes = doc.DocumentNode.SelectNodes("//*[@id='root']/div/div[2]/div[2]/div[1]/div/div[2]/div[5]/div/div/div[1]/div[1]/div/div");
			if (nodes != null) {
				for (int i = 0; i < nodes.Count; i++) {
					var selectNode = doc.DocumentNode.SelectSingleNode(string.Format("//*[@id='root']/div/div[2]/div[2]/div[1]/div/div[2]/div[5]/div/div/div[1]/div[1]/div/div[{0}]/div/span", i + 1));
					if (selectNode != null) {
						AddDataRow("옵션1[" + i + "]", selectNode.InnerText);
					}
				}
			}

			//옵션2차
			nodes = doc.DocumentNode.SelectNodes("//*[@id='root']/div/div[2]/div[2]/div[1]/div/div[2]/div[5]/div/div/div[1]/div[2]/div/div");
			if (nodes != null) {
				for (int i = 0; i < nodes.Count; i++) {
					var selectNode = doc.DocumentNode.SelectSingleNode(string.Format("//*[@id='root']/div/div[2]/div[2]/div[1]/div/div[2]/div[5]/div/div/div[1]/div[2]/div/div[{0}]/div/span", i + 1));
					if (selectNode != null) {
						AddDataRow("옵션2[" + (i + 1) + "]", selectNode.InnerText);
					}
				}
			}

			//Item상세정보
			nodes = doc.DocumentNode.SelectNodes("//*[@id=\"root\"]/div/div[2]/div[2]/div[3]/div[2]/div[1]/div/div[1]/div/div");
			if (nodes != null) {
				for (int i = 0; i < nodes.Count; i++) {
					var nodesSub = doc.DocumentNode.SelectNodes(string.Format("//*[@id='root']/div/div[2]/div[2]/div[3]/div[2]/div[1]/div/div[1]/div/div[{0}]/span", i + 1));
					if (nodesSub != null) {
						for (int j = 0; j < nodesSub.Count; j++) {
							var selectNode = doc.DocumentNode.SelectSingleNode(string.Format("//*[@id='root']/div/div[2]/div[2]/div[3]/div[2]/div[1]/div/div[1]/div/div[{0}]/span[{1}]", i + 1, j + 1));
							if (selectNode != null) {
								AddDataRow("Item상세정보[" + (i + 1) + "," + (j + 1) + "]", selectNode.InnerText);
							}
						}
					}
				}
			}

			//상세이미지정보
			////*[@id="root"]/div/div[2]/div[2]/div[3]/div[2]/div[1]/div/div[2]/div/div[1]/img
			nodes = doc.DocumentNode.SelectNodes("//*[@id='root']/div/div[2]/div[2]/div[3]/div[2]/div[1]/div/div[2]/div/div");
			if (nodes != null) {
				for (int i = 0; i < nodes.Count - 1; i++) {
					var selectNode = doc.DocumentNode.SelectSingleNode(string.Format("//*[@id='root']/div/div[2]/div[2]/div[3]/div[2]/div[1]/div/div[2]/div/div[{0}]/img", i + 1));
					if (selectNode != null) {
						AddDataRow("상세이미지[" + (i + 1) + "]", selectNode.Attributes["src"].Value);
					}
				}
			}
		}
		/// <summary>
		/// Taobao HTML Pasing Type A
		/// 테스트 상품 : 673983424836
		/// </summary>
		/// <param name="doc"></param>
		private void ParseHtmlToDataTableTypeA_All(HtmlAgilityPack.HtmlDocument doc) {
			string strData = "";
			HtmlAgilityPack.HtmlNodeCollection nodes;
			HtmlAgilityPack.HtmlNode selNode;
			DataRow dr;
			AddDataRow("Product Type", "A");
			//샵네임
			selNode = doc.DocumentNode.SelectSingleNode("//*[@id='root']/div/div[2]/div[1]/a/div[1]/div[1]/div");
			if (selNode != null) {
				AddDataRow("샵네임", selNode.InnerText);
			}

			//상품명
			selNode = doc.DocumentNode.SelectSingleNode("//*[@id='root']/div/div[2]/div[2]/div[1]/div/div[2]/div[1]/h1");
			if (selNode != null) {
				AddDataRow("상품명", selNode.InnerText);
			}
			//옵션1차
			nodes = doc.DocumentNode.SelectNodes("//*[@id='root']/div/div[2]/div[2]/div[1]/div/div[2]/div[5]/div/div/div[1]/div[1]/div/div");
			if (nodes != null) {
				for (int i = 0; i < nodes.Count; i++) {
					var selectNode = doc.DocumentNode.SelectSingleNode(string.Format("//*[@id='root']/div/div[2]/div[2]/div[1]/div/div[2]/div[5]/div/div/div[1]/div[1]/div/div[{0}]/div/span", i + 1));
					if (selectNode != null) {
						AddDataRow("옵션1[" + i + "]", selectNode.InnerText);
					}
				}
			}

			//옵션2차
			nodes = doc.DocumentNode.SelectNodes("//*[@id='root']/div/div[2]/div[2]/div[1]/div/div[2]/div[5]/div/div/div[1]/div[2]/div/div");
			if (nodes != null) {
				for (int i = 0; i < nodes.Count; i++) {
					var selectNode = doc.DocumentNode.SelectSingleNode(string.Format("//*[@id='root']/div/div[2]/div[2]/div[1]/div/div[2]/div[5]/div/div/div[1]/div[2]/div/div[{0}]/div/span", i + 1));
					if (selectNode != null) {
						AddDataRow("옵션2[" + (i + 1) + "]", selectNode.InnerText);
					}
				}
			}

			//Item상세정보
			nodes = doc.DocumentNode.SelectNodes("//*[@id=\"root\"]/div/div[2]/div[2]/div[3]/div[2]/div[1]/div/div[1]/div/div");
			if (nodes != null) {
				for (int i = 0; i < nodes.Count; i++) {
					var nodesSub = doc.DocumentNode.SelectNodes(string.Format("//*[@id='root']/div/div[2]/div[2]/div[3]/div[2]/div[1]/div/div[1]/div/div[{0}]/span", i + 1));
					if (nodesSub != null) {
						for (int j = 0; j < nodesSub.Count; j++) {
							var selectNode = doc.DocumentNode.SelectSingleNode(string.Format("//*[@id='root']/div/div[2]/div[2]/div[3]/div[2]/div[1]/div/div[1]/div/div[{0}]/span[{1}]", i + 1, j + 1));
							if (selectNode != null) {
								AddDataRow("Item상세정보[" + (i + 1) + "," + (j + 1) + "]", selectNode.InnerText);
							}
						}
					}
				}
			}

			//상세이미지정보
			////*[@id="root"]/div/div[2]/div[2]/div[3]/div[2]/div[1]/div/div[2]/div/div[1]/img
			nodes = doc.DocumentNode.SelectNodes("//*[@id='root']/div/div[2]/div[2]/div[3]/div[2]/div[1]/div/div[2]/div/div");
			if (nodes != null) {
				for (int i = 0; i < nodes.Count - 1; i++) {
					var selectNode = doc.DocumentNode.SelectSingleNode(string.Format("//*[@id='root']/div/div[2]/div[2]/div[3]/div[2]/div[1]/div/div[2]/div/div[{0}]/img", i + 1));
					if (selectNode != null) {
						AddDataRow("상세이미지[" + (i + 1) + "]", selectNode.Attributes["src"].Value);
					}
				}
			}


		}

		/// <summary>
		/// 현재 페이지의 html을 가져온다.
		/// </summary>
		private async void GetHtml() {
			JavascriptResponse response = await _chrome.EvaluateScriptAsync("document.documentElement.outerHTML");
			string htmlContent = "";
			if (response.Success && response.Result != null) {
				htmlContent = response.Result.ToString();
				var doc = new HtmlAgilityPack.HtmlDocument();
				doc.LoadHtml(htmlContent);
				txtHtml.Text = htmlContent;
			}
			else {
				MessageBox.Show("pasing 데이터 가져오기 실패_네트워크 오류");
			}
			tabRight.SelectedTab = tabHtml;
		}
		#endregion

		#region 기타기능
		private void AddRow(string strProduct) {
			DataRow drProduct;

			drProduct = dtProductList.NewRow();
			drProduct["num"] = dtProductList.Rows.Count + 1;
			drProduct["product_num"] = strProduct;
			drProduct["product_type"] = "";
			drProduct["complete"] = "";
			dtProductList.Rows.Add(drProduct);
		}

		private void AddDataRow(string strKey, string strValue) {

		}

		private void PrintStatusConsole(string strContext) {
			this.Invoke(new Action(delegate () {
				Console.WriteLine(strContext);
				statusLabel.Text = strContext;
			}));
		}

		#endregion

		#region Mac 주소 변경
		static string GetModifiedMac() {
			string strRandomNum = DateTime.Now.Ticks.ToString().Substring(0, 2);
			foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces()) {
				if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
					!nic.Description.ToLowerInvariant().Contains("virtual") &&
					!nic.Description.ToLowerInvariant().Contains("pseudo")) {
					string macAddress = nic.GetPhysicalAddress().ToString();
					Console.WriteLine("before MAC:" + macAddress);
					// MAC 주소의 마지막 숫자를 2로 변경
					if (macAddress.Length > 0) {
						macAddress = macAddress.Substring(0, macAddress.Length - 2) + strRandomNum;
					}
					Console.WriteLine("After MAC:" + macAddress);
					return macAddress;
				}
			}
			return null;
		}
		#endregion

		#region Button 이벤트
		/// <summary>
		/// Product List text 파일(*.txt) 가져오기
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGetProductList_Click(object sender, EventArgs e) {
			using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
				openFileDialog.InitialDirectory = "";
				openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
				openFileDialog.FilterIndex = 2;
				openFileDialog.RestoreDirectory = true;
				InitDataTable();
				if (openFileDialog.ShowDialog() == DialogResult.OK) {
					foreach (string strReadLine in File.ReadLines(openFileDialog.FileName)) {
						AddRow(strReadLine.Trim());
					}
					gridProductList.DataSource = dtProductList;
				}
			}
		}

		private void btnStart_Click(object sender, EventArgs e) {
			foreach (DataRow dr in dtProductList.Rows) {
				dr["complete"] = "";
			}
			layoutTop.Enabled = false;
			intDataRowNum = -1;
			PrintStatusConsole("자료수집 시작");
			CrawingProductStart();
			layoutTop.Enabled = true;
		}
		private void btnShowHtml_Click(object sender, EventArgs e) {
			GetHtml();
		}

		private void formCrawling_FormClosing(object sender, FormClosingEventArgs e) {
			_chrome.Dispose();
		}
		private void txtUrl_KeyPress(object sender, KeyPressEventArgs e) {
			if (e.KeyChar == (char)Keys.Enter) {
				_chrome.LoadUrl(txtUrl.Text);
			}
		}
		#endregion

		#region UnusaseCheck
		private void CheckBrowser() {
			Point WebLocation = _chrome.PointToScreen(new Point(0, 0));

			//(WebLocation.X, WebLocation.Y, _chrome.Size.Width, _chrome.Size.Height)
		}

		#endregion

		private void btnComapareImage_Click(object sender, EventArgs e) {
			//_chrome.CaptureScreenshotAsync(CefSharp.DevTools.Page.CaptureScreenshotFormat.Png);
			Stopwatch a = new Stopwatch();
			a.Start();
			Console.WriteLine(new clsCompareImage().CompareImage().ToString());
			a.Stop();
			Console.WriteLine(a.ElapsedMilliseconds);
		}

		private async void btnTest_Click(object sender, EventArgs e) {
			JavascriptResponse response = await _chrome.EvaluateScriptAsync("document.documentElement.outerHTML");
			if (response.Success && response.Result != null) {
				var doc = new HtmlAgilityPack.HtmlDocument();
				doc.LoadHtml(response.Result.ToString());
				ParseHtmlToJsonTypeTaobao(doc, response.Result.ToString());
			}

			/*
					//Replace와 정규식 속도차이 보자.....
			string strText = "<a href='javascript:;' style='background:url(//gd2.alicdn.com/imgextra/i3/3782733382/O1CN01Mxzkgq1ar074FBBTr_!!3782733382.jpg_30x30.jpg) center no-repeat;' data-spm-anchor-id='2013.1.iteminfo.5'>";
			string sss = "";

			Stopwatch watch = new Stopwatch();
			watch.Start();
			for(int i=0; i< 1000000; i++) {
				sss = strText.Replace(@"background:url(//", "").Replace("_30x30.jpg) center no-repeat;", "");
			}
			watch.Stop();
			long t1 = watch.ElapsedMilliseconds;

			watch.Start();
			for (int i = 0; i < 1000000; i++) {
				sss = FindPattern(strText, @"background:url\\(([^)]+)\\)");
			}
			watch.Stop();
			long t2 = watch.ElapsedMilliseconds;

			MessageBox.Show(string.Format("{0}-{1}", t1, t2));
			*/
		}
	}
}
