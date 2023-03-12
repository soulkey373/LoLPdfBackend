using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using System.Net;
using Microsoft.AspNetCore.Cors;
using Aspose.Pdf.Facades;
using LoLapi.Service;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Routing.Template;

namespace LoLapi.Controllers
{
	[EnableCors("AnotherPolicy")]
	[ApiController]
	[Route("[controller]")]
	public class GetLoLnameController : ControllerBase
	{
		[HttpGet(Name = "GetLoLname")]
		public IEnumerable<Result123> GetLoLname()
		{

			var url = "https://www.op.gg/champions?region=kr";
			var request = (HttpWebRequest)WebRequest.Create(url);
			List<Result123> result = new List<Result123>();
			Result123 result123 = new Result123();
			result123.Lolbusses = new List<lolbuss>();

			request.Headers.Add("Accept-Language", "zh-TW");
			var response = (HttpWebResponse)request.GetResponse();
			var stream = response.GetResponseStream();
			var doc = new HtmlDocument();
			doc.Load(stream);


			HtmlNodeCollection imgNode = doc.DocumentNode.SelectNodes("//nav[@class='css-1x3kezq e1y3xkpj1']//ul//li//a//img");
			HtmlNodeCollection imgNode1 = doc.DocumentNode.SelectNodes("//nav[@class='css-1x3kezq e1y3xkpj1']//ul//li//a//span");

			if (imgNode != null)
			{
				int resultcount = imgNode.Count();

				for (int x = 0; x < resultcount; x++)
				{
					var engName = imgNode[x].Attributes["alt"].Value.ToString().Trim();
					var chiName = imgNode1[x].InnerText.Trim();
					lolbuss lolbuss = new lolbuss();
					lolbuss.chinesename = chiName;
					lolbuss.engishname = engName;
					result123.Lolbusses.Add(lolbuss);

				}
			}
			result.Add(result123);
			return result;
		}


		[HttpPost(Name = "Generate_lol_report")]
		public string Generate_lol_report(List<Responsename> responsename)
		{
			try
			{
				int x = 0;
				string[] temp = new string[responsename.Count];
				var parameter = responsename;
				List<(double, double, string)> coordinates = new List<(double, double, string)>();
				List<(double, double)> refSystemCode = new List<(double, double)>();
				refSystemCode.Add((68.5, 545));
				refSystemCode.Add((204.3, 545));
				refSystemCode.Add((339.9, 545));
				refSystemCode.Add((475.8, 545));
				refSystemCode.Add((611.8, 545));
				refSystemCode.Add((68.5, 338.5));
				refSystemCode.Add((204.3, 338.5));
				refSystemCode.Add((339.9, 338.5));
				refSystemCode.Add((475.8, 338.5));
				refSystemCode.Add((611.8, 338.5));
				refSystemCode.Add((68.5, 132.7));
				refSystemCode.Add((204.3, 132.7));
				refSystemCode.Add((339.9, 132.7));
				refSystemCode.Add((475.8, 132.7));
				refSystemCode.Add((611.8, 132.7));

				foreach (Responsename result in parameter)
				{
					string input = result.name;
					string _pathResult = @$"app/image/{input}.jpg";
					if (System.IO.File.Exists(_pathResult)) { Console.WriteLine($"find {result.name} !"); }
					else { Console.WriteLine($"{result.name}not exists"); }
					coordinates.Add((refSystemCode[x].Item1, refSystemCode[x].Item2, _pathResult));
					string ExPdf = @$"app/outputPDF/lol0.pdf";
					string Nextpdf = @$"app/outputPDF/lol1.pdf";

					using (FileStream fs = new FileStream(Nextpdf, FileMode.Create, FileAccess.Write, FileShare.None))
					{
						PdfReader reader = new PdfReader(ExPdf);
						PdfStamper stamper = new PdfStamper(reader, fs);
						PdfContentByte canvas = stamper.GetOverContent(1);
						foreach ((double, double, string) pair in coordinates)
						{
							string filePath = pair.Item3;
							byte[] imageData = System.IO.File.ReadAllBytes(filePath);
							using (MemoryStream memoryStream = new MemoryStream(imageData))
							{
								iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(memoryStream);
								image.SetAbsolutePosition((float)pair.Item1, (float)pair.Item2);
								image.ScaleAbsoluteHeight(165);
								image.ScaleAbsoluteWidth(133);
								canvas.AddImage(image);
							}
						}
						stamper.Close();
						reader.Close();
					}
					x++;
				}
				string _path = @$"app/outputPDF/lol1.pdf";
				using (PdfReader reader = new PdfReader(_path))
				{

					using (MemoryStream ms = new MemoryStream())
					{

						using (PdfStamper stamper = new PdfStamper(reader, ms))
						{

						}
						string? bytms = Convert.ToBase64String(ms.ToArray());
						return bytms;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return "error:" + "\n" + ex.ToString();
			}

		}
	};
}
