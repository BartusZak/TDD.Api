using System;
using System.Collections.Generic;
using System.Text;

namespace TDD.Dto
{
    public class ResultDto<T> where T : BaseDto
    {
        public T SuccessResult { get; set; }
        public List<string> Errors { get; set; }
        public bool IsError => Errors?.Count > 0;
    }
}
