using System;

namespace Entities;

public interface IIdentifiable<ID>
{
    ID Id { get; set; }
}
