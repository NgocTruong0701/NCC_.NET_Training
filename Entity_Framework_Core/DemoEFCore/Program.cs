namespace DemoEFCore
{
    internal class Program
    {
        static void Main(string[] args)
        {
           using(var db = new BlogDbContext())
            {
                /*// Create
                Console.WriteLine("Inserting data");
                List<User> users = new List<User>();
                users.Add(new User { FirstName = "Le", LastName = "Ngoc Truong", BirthDate = DateTime.UtcNow, Role = UserRole.Admin});
                users.Add(new User { FirstName = "Le", LastName = "Duc Anh", BirthDate = DateTime.UtcNow , Role = UserRole.User});
                foreach(var u in users) {
                    db.Users.Add(u);
                }
               */
                db.Users.Add(new User { FirstName = "Ngoc", LastName = "Hoang Duc", BirthDate = DateTime.UtcNow, Role = UserRole.Guest });
                db.SaveChanges();
                // Read
                Console.WriteLine("Querying data");
                // get all data user
                var listUser = db.Users.ToList();
                // get user with userid = 1
                var user = db.Users.FirstOrDefault(u => u.FirstName.Contains("Ngoc"));
                // filter user data with FirstName = "Le"
                var listUser2 = db.Users.Where(u => u.FirstName.Contains("Le")).ToList();
                Console.WriteLine("Writing data");
                PrintUsers(listUser);
                PrintUser(user);
                PrintUsers(listUser2);

                // Update
                user.BirthDate = DateTime.Parse("07/01/2002");
                //db.Blogs.Add(new Blog { BlogName = "Tuoi tre 2", BlogUrl = "https://tuoitre.com.vn" });
                db.SaveChanges();
                Console.WriteLine("Update data successfully");
                Console.WriteLine("Writing data");
                PrintUsers(listUser);
                PrintUser(user);
                PrintUsers(listUser2);

                // Add Post
                /*db.Posts.Add(new Post { Title = "Title", CreateAt = DateTime.Now, BlogID = 14, UserId = 49 });
                db.Posts.Add(new Post { Title = "Title 1", CreateAt = DateTime.Now, BlogID = 14, UserId = 49 });*/
                /*db.Posts.Add(new Post { Title = "Title 5", CreateAt = DateTime.Now, BlogID = 15, UserId = 50 });
                db.Posts.Add(new Post { Title = "Title 6", CreateAt = DateTime.Now, BlogID = 15, UserId = 50 });
                db.SaveChanges();*/

                // Remove 
                db.Users.Remove(user);
                db.SaveChanges();
                Console.WriteLine("Remove data successfully");
                Console.WriteLine("Writing data");
                PrintUsers(listUser);
                PrintUser(user);
                PrintUsers(listUser2);

                // Lazy loading
                var userLazyLoading= db.Users.ToList();
                foreach (var us in userLazyLoading)
                {
                    Console.WriteLine($"User: {us.FirstName + " " + us.LastName} ");
                    if(us.Posts != null)
                    {
                        foreach (var post in us.Posts)
                        {
                            if (post != null)
                            {
                                Console.WriteLine($"- Post: {post.Title}");
                            }
                        }
                    }
                }

                Console.ReadKey();
            }
        }

        internal static void PrintUsers(List<User> userList)
        {
            foreach (var user in userList)
            {
                Console.WriteLine($"Id: {user.UserId}, Name: {user.FirstName} {user.LastName}, BirthDate: {user.BirthDate}, Role: {user.Role}");
            }
        }

        internal static void PrintUser(User user)
        {
            Console.WriteLine($"Id: {user.UserId}, Name: {user.FirstName} {user.LastName}, BirthDate: {user.BirthDate}, Role: {user.Role}");
        }
    }
}