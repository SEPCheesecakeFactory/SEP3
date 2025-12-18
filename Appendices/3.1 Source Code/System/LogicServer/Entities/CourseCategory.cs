using System;

namespace Entities;

public class CourseCategory : IIdentifiable<int>
{
    public int Id { set; get; }
    public string Name { set; get; }
    public string Descritpion{ set; get; }
}
