using API_1;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client_API_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GetStudentAsync().Wait();
            Console.ReadKey();
        }

        static async Task GetStudentAsync()
        {
            // Cấu hình
            using var client = new HttpClient { BaseAddress = new Uri("https://localhost:7009") };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("/student");
            if (response.IsSuccessStatusCode)
            {
                // Đọc dữ liệu trả về thành danh sách (List) của đối tượng Student
                List<Student> students = await response.Content.ReadFromJsonAsync<List<Student>>();

                // Xử lý dữ liệu
                foreach (Student student in students)
                {
                    Console.WriteLine($"ID: {student.Id}, Name: {student.FirstName + " " + student.LastName }, Age: {student.Age}, MathScores: {student.MathScores}, EnglishScores: {student.EnglishScores}, ChemistryScores: {student.ChemistryScores}, TotalScores: {student.MathScores + student.EnglishScores + student.ChemistryScores}");
                }
            }
            else
            {
                Console.WriteLine("Lỗi trong quá trình gọi API.");
            }
        }
    }
}