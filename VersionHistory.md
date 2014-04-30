Version History
=

30th April 2014
-

- Supports version 1.1.40428.1602 of the JobServe Jobs API
- Added common data service abstractions and implementations (ICountryService/CountryService for example)
- Renamed JobServeAPIClient class to WebRequestManager to more accurately reflect it's function
- Added the Client class and IClient interface, which offers more object-centric methods for getting data
- Project is now built as a PCL targeting Windows Store, Silverlight 5, WP8 and Xamarin Android/iOS in addition .Net 4.5
- Loads more unit tests
- (Almost!) Fully documented.