using DistributedCaching.Model;
using DistributedCaching.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DistributedCaching.Controllers
{
    [Route("api/caching")]
    [ApiController]
    public class CachingController : ControllerBase
    {

        private IConfiguration Configuration;
        private readonly IMemoryCache mCache;

        public CachingController(IConfiguration configuration, IMemoryCache memoryCache)
        {
            Configuration = configuration;
            mCache = memoryCache;
        }

        [HttpGet("")]
        public ActionResult GetResult()
        {
            string cString = Configuration.GetConnectionString("SqlServerCString");

            List<Student> student = new List<Student>();

            student = mCache.getCache<List<Student>>("student");

            if (student == null)
            {
                using (SqlConnection con = new SqlConnection(cString))
                {
                    con.Open();

                    List<Student> studentList = new List<Student>();

                    string query = "select * from Student";

                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader sqlDataReader = cmd.ExecuteReader();

                    while (sqlDataReader.Read())
                    {
                        studentList.Add(new Student()
                        {
                            Id = (int)sqlDataReader["Id"],
                            Name = (string)sqlDataReader["Name"],
                            Age = (int)sqlDataReader["Age"]
                        });
                    }

                    mCache.setCache<List<Student>>(studentList, "student");

                    return Ok(studentList);

                }
            }
            return Ok(student);
        }

    }
}
