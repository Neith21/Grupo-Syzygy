
namespace SyzygyVeterinaryAPIControllersData.Services.AnalisysExams
{
	public interface IAnalisysExamService
	{
		Task<List<string>> AnalisysExams(int examID);
	}
}