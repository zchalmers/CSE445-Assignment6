// IService1.cs - Service Contract Interface
// Project: WebAndFilterService (WCF Service Application)
// Author: Andrew Courter
// Course: CSE445 - Distributed Software Development
// 
//
// Description: Defines the WCF service contract for:
//   - WebDownload (Required - WSDL): Downloads webpage content from a URL
//   - WordFilter  (Required - WSDL): Filters stop words and HTML/XML tags


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Assignment6
{
    
    [ServiceContract]
    public interface IService1
    {
        // REQUIRED SERVICE 1: WebDownload

        
        /// <param name="url">A valid HTTP or HTTPS URL to download</param>
       
        /// The full content of the webpage as a string.
        /// If an error occurs, returns a string starting with "Error:" 
       
      
        [OperationContract]
        string WebDownload(string url);

        
        // REQUIRED SERVICE 2: WordFilter

        
        /// <param name="text">The input text to filter</param>
        
        /// A cleaned string with stop words and HTML/XML tags removed.
        /// Only content-bearing words remain, separated by single spaces.
       
        [OperationContract]
        string WordFilter(string text);
    }
}
