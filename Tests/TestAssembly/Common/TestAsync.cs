using System.Threading.Tasks;

namespace TestAssembly
{
    public class TestAsync
    {
        public static async void TestDelay()
        {
            await Task.Delay(500);
        }
    }
}
