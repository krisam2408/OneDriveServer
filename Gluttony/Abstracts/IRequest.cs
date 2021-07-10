using Gluttony.DataTransfer;
using Gluttony.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gluttony.Abstracts
{
    public abstract class IRequest<T> where T : Response
    {
        public string Command { get; set; }
        public Dictionary<string, string> Parameters { get; protected set; }
        public Func<HttpResponseMessage, Task<T>> SuccessCallBack { get; init; }
        public Func<HttpResponseMessage, Task<T>> ErrorCallBack { get; init; }
        public CultureInfo Culture { get; protected set; }

        protected IRequest(Func<HttpResponseMessage, Task<T>> success)
        {
            Culture = new("en-US");
            Parameters = new();
            SuccessCallBack = success;
            ErrorCallBack = null;
        }

        protected IRequest(Func<HttpResponseMessage, Task<T>> success, Func<HttpResponseMessage, Task<T>> error) : this(success)
        {
            ErrorCallBack = error;
        }

        protected IRequest(Func<HttpResponseMessage, Task<T>> success, CultureInfo culture) : this(success)
        {
            Culture = culture;
        }

        protected IRequest(Func<HttpResponseMessage, Task<T>> success, Func<HttpResponseMessage, Task<T>> error, CultureInfo culture) : this(success, error)
        {
            Culture = culture;
        }

        public async Task AddParameter(string parameterName, object value, ParameterTreatment treatAs = ParameterTreatment.String)
        {
            string parameter;
            try
            {
                switch (treatAs)
                {
                    case ParameterTreatment.Date:
                        DateTime date = (DateTime)value;
                        parameter = date.ToString(Culture.DateTimeFormat);
                        break;
                    case ParameterTreatment.FilePath:
                        parameter = await ProcessingFilePathsAsync(value);
                        break;
                    case ParameterTreatment.ArrayOfStrings:
                        string[] strList = (string[]) value;
                        parameter = JsonConvert.SerializeObject(strList);
                        break;
                    case ParameterTreatment.ArrayOfNumbers:
                        int[] numList = (int[])value;
                        parameter = JsonConvert.SerializeObject(numList);
                        break;
                    case ParameterTreatment.ArrayOfDates:
                        DateTime[] dateList = (DateTime[])value;
                        parameter = JsonConvert.SerializeObject(dateList);
                        break;
                    case ParameterTreatment.ArrayOfFilePaths:
                        string[] fileList = (string[])value;
                        List<string> paths = new();
                        foreach (string path in fileList)
                            paths.Add(await ProcessingFilePathsAsync(path));

                        parameter = JsonConvert.SerializeObject(paths);
                        break;
                    case ParameterTreatment.Number:
                    case ParameterTreatment.String:
                        parameter = value.ToString();
                        break;
                    case ParameterTreatment.Object:
                    default:
                        parameter = JsonConvert.SerializeObject(value);
                        break;
                }
            }
            catch(Exception e)
            {
                throw new IncorrectTreatmentException(treatAs, e);
            }

            Parameters.Add(parameterName, parameter);
        }

        private static async Task<string> ProcessingFilePathsAsync(object filepath)
        {
            using FileStream fs = new(filepath.ToString(), FileMode.Open);
            using MemoryStream ms = new();
            await fs.CopyToAsync(ms);
            byte[] buffer = ms.ToArray();
            return Convert.ToBase64String(buffer);
        }
    }
}
