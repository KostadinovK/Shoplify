using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shoplify.Services.Mapping;

namespace Shoplify.Services
{
   

    public class TestService : ITestService
    {
        public void TestAutoMapper()
        {
            var list = new List<Person>();
            for (int i = 0; i < 5; i++)
            {
                list.Add(new Person()
                {
                    Id = $"{i}Id",
                    Name = "Goshko",
                    Age = (i + 1) * 7
                });
            }

            var peoples = list.OrderByDescending(p => p.Age).AsQueryable().To<PersonDto>().ToList();
        }
    }
}
