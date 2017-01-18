
//ViewModel
public class MyCountry
{
	public string ID { get; set; }
	public string Name { get; set; }
	public List<MyCategory> CategoryList { get; set; }
}

public class MyCategory
{
	public int ID { get; set; }
	public string Name { get; set; }
	public List<MyStore> StoreList { get; set; }
}

public class MyStore
{
	public int CatID { get; set; }
	public string CouID { get; set; }
	public string Name { get; set; }
	public string Tel { get; set; }
}

public class InputModel
{
	public string CouID { get; set; }
	public int CatID { get; set; }
}

public class OutputModel
{
	public bool IsSuccess { get; set; }
	public int ErrorType { get; set; }
	public string ErrorMsg { get; set; }
	public string CatJsonStr { get; set; }
	public string StoreJsonStr { get; set; }
}

//Controller.cs
List<MyCountry> MyCountryList = new List<MyCountry>()
{
	new MyCountry() { ID = "tw", Name = "台灣"},
	new MyCountry() { ID = "us", Name = "美國"},
	new MyCountry() { ID = "jp", Name = "日本"},
};

ViewBag.MyCountrysList = new SelectList(MyCountryList, "ID", "Name");


//API & functions
[HttpPost]
[Route("ApiCommon/GetCatList")]
public string GetCatList(InputModel inputModel)
{
	string testCouID = inputModel.CouID;
	int testCatID = inputModel.CatID;

	OutputModel outputModel = new OutputModel();

	List<MyCountry> MyCountryList = null;
	List<MyCategory> MyCategoryList = null;
	List<MyStore> MyStoreList = null;
	GetData(ref MyCountryList, ref MyCategoryList, ref MyStoreList);

	if (testCouID == null || testCouID == "")
	{
		outputModel.IsSuccess = false;
		outputModel.ErrorMsg = "Country 沒選";
		outputModel.ErrorType = 1;

		return JsonConvert.SerializeObject(outputModel);
	}

	List<MyCategory> tmpCategoryList = GetCategoryList(testCouID, MyStoreList, MyCategoryList);

	if (testCatID == 0)
	{
		outputModel.IsSuccess = false;
		outputModel.ErrorMsg = "Category 沒選";
		outputModel.CatJsonStr = JsonConvert.SerializeObject(tmpCategoryList);
		outputModel.ErrorType = 2;

		return JsonConvert.SerializeObject(outputModel);
	}

	List<MyStore> tempList = MyStoreList.Where(w => w.CouID == testCouID && w.CatID == testCatID).ToList();

	outputModel.IsSuccess = true;
	outputModel.ErrorMsg = "";
	outputModel.CatJsonStr = JsonConvert.SerializeObject(MyCategoryList);
	outputModel.StoreJsonStr = JsonConvert.SerializeObject(tempList);

	return JsonConvert.SerializeObject(outputModel);
}

private void GetData(ref List<MyCountry> MyCountryList, ref List<MyCategory> MyCategoryList, ref List<MyStore> MyStoreList)
{
	MyCountryList = new List<MyCountry>()
	{
		new MyCountry() { ID = "tw", Name = "台灣"},
		new MyCountry() { ID = "us", Name = "美國"},
		new MyCountry() { ID = "jp", Name = "日本"},
	};

	MyCategoryList = new List<MyCategory>()
	{
		new MyCategory() { ID = 2, Name="主機板" },
		new MyCategory() { ID = 3, Name="顯示卡" },
		new MyCategory() { ID = 5, Name="筆記型電腦" },
	};

	MyStoreList = new List<MyStore>()
	{
		new MyStore() {  CouID = "tw", CatID = 2, Name = "店家 A", Tel = "111" },
		new MyStore() {  CouID = "us", CatID = 3 ,Name = "店家 B", Tel = "222" },
		new MyStore() {  CouID = "jp", CatID = 5 ,Name = "店家 C", Tel = "333" },
		new MyStore() {  CouID = "tw", CatID = 2 ,Name = "店家 D", Tel = "444" },
		new MyStore() {  CouID = "us", CatID = 2 ,Name = "店家 E", Tel = "555" },
		new MyStore() {  CouID = "jp", CatID = 3 ,Name = "店家 F", Tel = "666" },
		new MyStore() {  CouID = "tw", CatID = 3 ,Name = "店家 G", Tel = "777" },
		new MyStore() {  CouID = "us", CatID = 5 ,Name = "店家 H", Tel = "888" },
	};
}

private List<MyCategory> GetCategoryList(string testCouID, List<MyStore> MyStoreList, List<MyCategory> MyCategoryList)
{
	string tmpCouID = string.Empty;
	List<MyStore> tmpStoryList = null;
	List<int> tmpCatIDList = null;
	List<MyCategory> tmpCategoryList = null;
	tmpCouID = testCouID;
	tmpStoryList = MyStoreList.Where(w => w.CouID == tmpCouID).ToList();
	tmpCatIDList = tmpStoryList.Select(s => s.CatID).Distinct().ToList();
	tmpCategoryList = MyCategoryList.Where(w => tmpCatIDList.Contains(w.ID)).ToList();

	return tmpCategoryList;
}