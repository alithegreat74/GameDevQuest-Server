using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace GamedevQuest.Models
{
    public class OperationResult<T>
    {
        public T? Result { get; private set; }
        public IActionResult? ActionResultObject { get; private set; }

        public OperationResult(IActionResult resultType)
        {
            Result = default(T);
            ActionResultObject = resultType;
        }
        public OperationResult(T result)
        {
            Result = result;
            ActionResultObject = null;
        }
    }
}
