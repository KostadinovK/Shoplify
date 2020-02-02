using System;
using System.Collections.Generic;
using System.Text;
using Shoplify.Services.Mapping;

namespace Shoplify.Services
{
    public class PersonDto : IMapFrom<Person>
    {
        public string Id { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
    }
}
