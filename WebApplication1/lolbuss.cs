namespace LoLapi
{
	public class lolbuss
	{
		public string chinesename { get; set; }

		public string engishname { get; set; }
	}
	public class Result123
	{
		public  List<lolbuss> Lolbusses { get; set; }

	}

	public class Responsename
	{
		public string name { get; set; }
	}
	public class ResRoot
	{
		public List<Responsename>? getres { get; set; }
	}

}
