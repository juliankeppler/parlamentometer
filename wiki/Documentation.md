<a name='assembly'></a>
# sweproject

## Contents

- [DIP](#T-sweproject-DIP 'sweproject.DIP')
  - [#ctor()](#M-sweproject-DIP-#ctor 'sweproject.DIP.#ctor')
  - [GetMentions(term,electionPeriods)](#M-sweproject-DIP-GetMentions-System-String,System-Int32[]- 'sweproject.DIP.GetMentions(System.String,System.Int32[])')
  - [GetRelevance(term,mode,electionPeriods)](#M-sweproject-DIP-GetRelevance-System-String,sweproject-GroupMode,System-Int32[]- 'sweproject.DIP.GetRelevance(System.String,sweproject.GroupMode,System.Int32[])')
  - [GetRelevance(term,mode)](#M-sweproject-DIP-GetRelevance-System-String,sweproject-GroupMode- 'sweproject.DIP.GetRelevance(System.String,sweproject.GroupMode)')
  - [GetResults(term,electionPeriods)](#M-sweproject-DIP-GetResults-System-String,System-Int32[]- 'sweproject.DIP.GetResults(System.String,System.Int32[])')
  - [GetResults(term)](#M-sweproject-DIP-GetResults-System-String- 'sweproject.DIP.GetResults(System.String)')
  - [Request(args)](#M-sweproject-DIP-Request-System-Collections-Specialized-NameValueCollection- 'sweproject.DIP.Request(System.Collections.Specialized.NameValueCollection)')
  - [SetDefaultParams(term,electionPeriods)](#M-sweproject-DIP-SetDefaultParams-System-String,System-Int32[]- 'sweproject.DIP.SetDefaultParams(System.String,System.Int32[])')
  - [ToQueryString(args)](#M-sweproject-DIP-ToQueryString-System-Collections-Specialized-NameValueCollection- 'sweproject.DIP.ToQueryString(System.Collections.Specialized.NameValueCollection)')
- [GroupMode](#T-sweproject-GroupMode 'sweproject.GroupMode')
  - [Month](#F-sweproject-GroupMode-Month 'sweproject.GroupMode.Month')
  - [Year](#F-sweproject-GroupMode-Year 'sweproject.GroupMode.Year')
- [Plotter](#T-sweproject-Plotter 'sweproject.Plotter')
  - [Plot()](#M-sweproject-Plotter-Plot-System-String,System-Collections-Generic-SortedDictionary{System-String,System-Int32},sweproject-GroupMode- 'sweproject.Plotter.Plot(System.String,System.Collections.Generic.SortedDictionary{System.String,System.Int32},sweproject.GroupMode)')
- [Program](#T-sweproject-Program 'sweproject.Program')
  - [GetUserInput()](#M-sweproject-Program-GetUserInput-sweproject-DIP- 'sweproject.Program.GetUserInput(sweproject.DIP)')

<a name='T-sweproject-DIP'></a>
## DIP `type`

##### Namespace

sweproject

##### Summary

This class provides methods for gathering data from the DIP-API.

<a name='M-sweproject-DIP-#ctor'></a>
### #ctor() `constructor`

##### Summary

Initializes a new instance of [DIP](#T-sweproject-DIP 'sweproject.DIP').

##### Parameters

This constructor has no parameters.

<a name='M-sweproject-DIP-GetMentions-System-String,System-Int32[]-'></a>
### GetMentions(term,electionPeriods) `method`

##### Summary

Gets a List containing the dates of each mention of the search term.

##### Returns

A [List\`1](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List`1 'System.Collections.Generic.List`1') of dates for each time the term was mentioned. Dates may appear multiple times when the term was mention more than once on a given day.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| term | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') representing the search term. |
| electionPeriods | [System.Int32[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32[] 'System.Int32[]') | A [int[]](#T-int[] 'int[]') containing the selected election periods. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentException 'System.ArgumentException') | `term` is an invalid search term. |
| [System.InvalidOperationException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.InvalidOperationException 'System.InvalidOperationException') | There are too many results for `term` during `electionPeriods`. |

<a name='M-sweproject-DIP-GetRelevance-System-String,sweproject-GroupMode,System-Int32[]-'></a>
### GetRelevance(term,mode,electionPeriods) `method`

##### Summary

Groups all mentions of a search term by time.

##### Returns

A [SortedDictionary\`2](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.SortedDictionary`2 'System.Collections.Generic.SortedDictionary`2') containing the amount of mentions of the `term` grouped by time.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| term | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') representing the search term. |
| mode | [sweproject.GroupMode](#T-sweproject-GroupMode 'sweproject.GroupMode') | A [GroupMode](#T-sweproject-GroupMode 'sweproject.GroupMode') determining whether the results should be grouped by Month or Year. |
| electionPeriods | [System.Int32[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32[] 'System.Int32[]') | An [int[]](#T-int[] 'int[]') containing the selected election periods. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentException 'System.ArgumentException') | `term` is an invalid search term. |
| [System.InvalidOperationException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.InvalidOperationException 'System.InvalidOperationException') | There are too many results for `term` during `electionPeriods`. |

<a name='M-sweproject-DIP-GetRelevance-System-String,sweproject-GroupMode-'></a>
### GetRelevance(term,mode) `method`

##### Summary

Groups all mentions of a search term by time.

##### Returns

A [Dictionary\`2](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.Dictionary`2 'System.Collections.Generic.Dictionary`2') containing the amount of mentions of the `term` grouped by time.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| term | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') representing the search term. |
| mode | [sweproject.GroupMode](#T-sweproject-GroupMode 'sweproject.GroupMode') | A [GroupMode](#T-sweproject-GroupMode 'sweproject.GroupMode') determining whether the results should be grouped by Month or Year. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentException 'System.ArgumentException') | `term` is an invalid search term. |
| [System.InvalidOperationException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.InvalidOperationException 'System.InvalidOperationException') | There are too many results for `term`. |

<a name='M-sweproject-DIP-GetResults-System-String,System-Int32[]-'></a>
### GetResults(term,electionPeriods) `method`

##### Summary

Retrieves the number of results found for a given search term.

##### Returns

An [Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') representing the number of results found.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| term | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') representing the search term. |
| electionPeriods | [System.Int32[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32[] 'System.Int32[]') | A [int[]](#T-int[] 'int[]') containing the selected election periods. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentException 'System.ArgumentException') | `term` is an invalid search term. |

<a name='M-sweproject-DIP-GetResults-System-String-'></a>
### GetResults(term) `method`

##### Summary

Retrieves the number of results found for a given search term.

##### Returns

An [Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') representing the number of results found.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| term | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') representing the search term. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentException 'System.ArgumentException') | `term` is an invalid search term. |

<a name='M-sweproject-DIP-Request-System-Collections-Specialized-NameValueCollection-'></a>
### Request(args) `method`

##### Summary

Performs an http request to the DIP API.

##### Returns

A [](#N-System-Dynamic 'System.Dynamic') object representing the response of the API.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| args | [System.Collections.Specialized.NameValueCollection](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Specialized.NameValueCollection 'System.Collections.Specialized.NameValueCollection') | A [NameValueCollection](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Specialized.NameValueCollection 'System.Collections.Specialized.NameValueCollection') holding the query arguments for the request. |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentException 'System.ArgumentException') | `args` holds invalid parameters. |

<a name='M-sweproject-DIP-SetDefaultParams-System-String,System-Int32[]-'></a>
### SetDefaultParams(term,electionPeriods) `method`

##### Summary

Helper method that sets the standard parameters of a query string.

##### Returns

A [NameValueCollection](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Specialized.NameValueCollection 'System.Collections.Specialized.NameValueCollection') holding the query args.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| term | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | A [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') representing the search term. |
| electionPeriods | [System.Int32[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32[] 'System.Int32[]') | A [int[]](#T-int[] 'int[]') containing the selected election periods. |

<a name='M-sweproject-DIP-ToQueryString-System-Collections-Specialized-NameValueCollection-'></a>
### ToQueryString(args) `method`

##### Summary

Converts a given NameValueCollection with query arguments into a query string.

##### Returns

A query [String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String').

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| args | [System.Collections.Specialized.NameValueCollection](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Specialized.NameValueCollection 'System.Collections.Specialized.NameValueCollection') | The [NameValueCollection](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Specialized.NameValueCollection 'System.Collections.Specialized.NameValueCollection') holding the query arguments as key, value pairs. |

<a name='T-sweproject-GroupMode'></a>
## GroupMode `type`

##### Namespace

sweproject

##### Summary

GroupMode provides different options for tallying up the results.

<a name='F-sweproject-GroupMode-Month'></a>
### Month `constants`

##### Summary

Results are grouped by Month.

<a name='F-sweproject-GroupMode-Year'></a>
### Year `constants`

##### Summary

Results are grouped by Year.

<a name='T-sweproject-Plotter'></a>
## Plotter `type`

##### Namespace

sweproject

##### Summary

This class provides methods for plotting the final diagram.

<a name='M-sweproject-Plotter-Plot-System-String,System-Collections-Generic-SortedDictionary{System-String,System-Int32},sweproject-GroupMode-'></a>
### Plot() `method`

##### Summary

Generates a diagram and saves it as a file.

##### Parameters

This method has no parameters.

<a name='T-sweproject-Program'></a>
## Program `type`

##### Namespace

sweproject

<a name='M-sweproject-Program-GetUserInput-sweproject-DIP-'></a>
### GetUserInput() `method`

##### Summary

Prompts user to input search term and search time frame.

##### Returns

A ([String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String'), [int[]](#T-int[] 'int[]')) tuple holding a search term and an array of election periods. The string is not empty. The array may be empty.

##### Parameters

This method has no parameters.
