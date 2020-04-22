// Copyright (c) 2020 Carl Reinke
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Sdk;

namespace ItsyBitsy.Xunit
{
    /// <summary>
    /// Like <see cref="MemberDataAttribute"/> but takes <see cref="ITuple"/> instead of an
    /// <see cref="object"/> array.
    /// </summary>
    /// <seealso cref="MemberDataAttribute"/>
    [DataDiscoverer("Xunit.Sdk.MemberDataDiscoverer", "xunit.core")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public sealed class TupleMemberDataAttribute : MemberDataAttributeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TupleMemberDataAttribute"/> class.
        /// </summary>
        /// <param name="memberName">The name of the public static member on the test class that
        ///     will provide the test data.</param>
        /// <param name="parameters">The parameters for the member if it is a method.</param>
        public TupleMemberDataAttribute(string memberName, params object[] parameters)
            : base(memberName, parameters)
        {
        }

        /// <inheritdoc/>
        protected override object?[]? ConvertDataItem(MethodInfo testMethod, object? item)
        {
            if (item == null)
                return null;

            var tuple = item as ITuple;
            if (tuple == null)
                throw new ArgumentException($"Property {MemberName} on {MemberType ?? testMethod?.DeclaringType} yielded an item that is not an ITuple.");

            var objs = new object?[tuple.Length];
            for (int i = 0; i < objs.Length; ++i)
                objs[i] = tuple[i];
            return objs;
        }
    }
}
