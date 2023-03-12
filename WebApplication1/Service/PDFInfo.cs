namespace LoLapi.Service
{
	public class PDFInfo
	{
		public bool deleFile()
		{
			try
			{
				//string parentDirectory2 = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName).FullName).FullName).FullName;

				for (int i = 1; i < 14; i++)
				{
					string Pdf = @$"app/outputPDF/lol{i + 1}.pdf";
					if (File.Exists(Pdf))
					{
						Console.WriteLine("PDF:{0}存在",Pdf);
						File.Delete(Pdf);
					}
				}
				
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("刪除PDF失敗: {0}" ,ex.Message);
				return false;
			}

		}
	}

}
