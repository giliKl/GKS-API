﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKS.Core.Shared
{
    public  class  Responses<T>
    {
      
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; }

        // Success response
        public static Responses<T> Success(T data)
        {
            return new Responses<T>
            {
                IsSuccess = true,
                Data = data,
                StatusCode = 200 // OK
            };
        }

        // Failure response with a custom message
        public static Responses<T> Failure(string errorMessage, int statusCode = 400)
        {
            return new Responses<T>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                StatusCode = statusCode
            };
        }

        // Utility for NotFound response
        public static Responses<T> NotFound(string errorMessage = "Not Found")
        {
            return new Responses<T>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                StatusCode = 404
            };
        }

        // Utility for BadRequest response
        public static Responses<T> BadRequest(string errorMessage = "Bad Request")
        {
            return new Responses<T>
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                StatusCode = 400
            };
        }
    }
}
