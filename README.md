# Cascading Configuration Sources

The purpose of this library is to enable an application to rapidly specify 1 or numerous configuration sources, allowing for debugging, overriding values, performing in place hotfixes, and many many more uses.

There are three levels that users of this library should concern themselves with:

* Config:
This object is created by the user of the library. It should inherit from IConfig. 

* ConfigSource:
This object specifies exactly how the aforementioned Config gets its values. Several sources are defined out of the box such as 

  * IDbConfigSource:
    This class can be extended to allow for easy access to config values kept in a database. You might specify a                               Dictionary<string(PropertyName),string(SqlString)> wherein each property is populated by the accompanying SQL string.
    
     * KvpDbConfigSource:
     Populates your specified config with values from the a dbo.table where the values are all in the same column and identified by their        property name in the database.
     
      *  SingleRecordDbConfigSource:
       Populates your specified config with values are all all sourced from a single record in a database. The property values in your            config should have names which match the columns in the table you are sourcing from.
       
  * AppConfigSource
    Mostly in place to allow for you to place values sourced using the Configuration Manager to be placed within the Hierarchy of Cascading     Configuration Sources. The keys specified in the .config file should match the name of the properties in your Config class.
    
  * TextFileConfigSource
    Works very similarly to AppConfigSource in that it matches on key-value pairs where the properties match the keys specified in the .txt     file. It sources these keys the text file provided during the construction of this object.
    
* ConfigProvider:
This object holds a sorted HashSet of the ConfigSources you've specified. Iterating over the sources to populate the 

This object wraps the config file you've specified. Providing utility methods to the ConfigSources for setting and converting properties in the IConfig object. You specify the source such as other objects in memory, remote resources pulled via http requests, yaml files, etc. 

Call typeOf(Config).GetProperties to retrieve the properties specified in your custom type. Then call Config.Provider.SetProperty(property, value) for each. 

This object also has some properties of its own:
  * Cascade:
    This property, when true, will allow for iterating over multiple ConfigSources. Starting with the lowest priority objects and moving     progressively higher. The result being that values from higher priority sources overwriting that of lower priority sources.


Further description to come....
