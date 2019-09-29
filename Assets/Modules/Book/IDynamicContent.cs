using System;
using Modules.Common;

namespace Modules.Book {

    public interface IDynamicContent {

        T Content<T>();

        Type ContentClassType();

        ContentType ContentType();

    }
}