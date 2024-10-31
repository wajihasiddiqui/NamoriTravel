using System.Xml.Serialization;

namespace ModelsDTO
{
    //[XmlRoot(ElementName = "result")]
    //public class Result
    //{
    //    [XmlElement(ElementName = "currencyShort")]
    //    public string CurrencyShort { get; set; } = null!; // Default to not-nullable

    //    [XmlElement(ElementName = "hotels")]
    //    public Hotels Hotels { get; set; } = new Hotels(); // Default to new Hotels object

    //    [XmlElement(ElementName = "successful")]
    //    public bool Successful { get; set; }

    //    [XmlAttribute(AttributeName = "command")]
    //    public string Command { get; set; } = null!; // Default to not-nullable

    //    [XmlAttribute(AttributeName = "tID")]
    //    public string TID { get; set; } = null!; // Default to not-nullable

    //    [XmlAttribute(AttributeName = "ip")]
    //    public string Ip { get; set; } = null!; // Default to not-nullable

    //    [XmlIgnore]
    //    [XmlAttribute("date")]
    //    public string Date { get; set; } = null!; // Default to not-nullable

    //    [XmlAttribute(AttributeName = "version")]
    //    public double Version { get; set; }

    //    [XmlAttribute(AttributeName = "elapsedTime")]
    //    public double ElapsedTime { get; set; }
    //}

    //public class Hotels
    //{
    //    [XmlAttribute(AttributeName = "count")]
    //    public int Count { get; set; }

    //    [XmlElement(ElementName = "hotel")]
    //    public List<Hotel> HotelList { get; set; } = new List<Hotel>(); // Initialize as empty list
    //}

    //public class Hotel
    //{
    //    [XmlAttribute(AttributeName = "runno")]
    //    public int RunNo { get; set; }

    //    [XmlAttribute(AttributeName = "preferred")]
    //    public string? Preferred { get; set; } // Nullable

    //    [XmlAttribute(AttributeName = "exclusive")]
    //    public string? Exclusive { get; set; } // Nullable

    //    [XmlAttribute(AttributeName = "cityname")]
    //    public string? CityName { get; set; } // Nullable

    //    [XmlAttribute(AttributeName = "hotelid")]
    //    public string? HotelId { get; set; } // Nullable

    //    [XmlElement(ElementName = "preferred")]
    //    public string? PreferredYes { get; set; } // Nullable

    //    [XmlElement(ElementName = "builtYear")]
    //    public int? BuiltYear { get; set; } // Nullable

    //    [XmlElement(ElementName = "renovationYear")]
    //    public int? RenovationYear { get; set; } // Nullable

    //    [XmlElement(ElementName = "floors")]
    //    public int? Floors { get; set; } // Nullable

    //    [XmlElement(ElementName = "noOfRooms")]
    //    public int NoOfRooms { get; set; }

    //    [XmlElement(ElementName = "fullAddress")]
    //    public FullAddress FullAddress { get; set; } = new FullAddress(); // Default to new FullAddress object

    //    [XmlElement(ElementName = "description1")]
    //    public Description1 Description1 { get; set; } = new Description1(); // Default to new Description1 object

    //    [XmlElement(ElementName = "hotelName")]
    //    public string? HotelName { get; set; } // Nullable

    //    [XmlElement(ElementName = "location1")]
    //    public string? Location1 { get; set; } // Nullable

    //    [XmlElement(ElementName = "countryCode")]
    //    public int CountryCode { get; set; }

    //    [XmlElement(ElementName = "regionName")]
    //    public string? RegionName { get; set; } // Nullable

    //    [XmlElement(ElementName = "amenitie")]
    //    public Amenitie Amenitie { get; set; } = new Amenitie(); // Default to new Amenitie object

    //    [XmlElement(ElementName = "leisure")]
    //    public Leisure Leisure { get; set; } = new Leisure(); // Default to new Leisure object

    //    [XmlElement(ElementName = "business")]
    //    public Business Business { get; set; } = new Business(); // Default to new Business object

    //    [XmlElement(ElementName = "transportation")]
    //    public Transportation Transportation { get; set; } = new Transportation(); // Default to new Transportation object

    //    [XmlElement(ElementName = "hotelPhone")]
    //    public string? HotelPhone { get; set; } // Nullable

    //    [XmlElement(ElementName = "hotelCheckIn")]
    //    public string? HotelCheckIn { get; set; } // Nullable

    //    [XmlElement(ElementName = "hotelCheckOut")]
    //    public string? HotelCheckOut { get; set; } // Nullable

    //    [XmlElement(ElementName = "rating")]
    //    public int Rating { get; set; }

    //    [XmlElement(ElementName = "fireSafety")]
    //    public string? FireSafety { get; set; } // Nullable

    //    [XmlElement(ElementName = "geoPoint")]
    //    public GeoPoint GeoPoint { get; set; } = new GeoPoint(); // Default to new GeoPoint object

    //    [XmlElement(ElementName = "rooms")]
    //    public Rooms Rooms { get; set; } = new Rooms(); // Default to new Rooms object
    //}

    //public class FullAddress
    //{
    //    [XmlElement(ElementName = "hotelStreetAddress")]
    //    public string? HotelStreetAddress { get; set; } // Nullable

    //    [XmlElement(ElementName = "hotelZipCode")]
    //    public string? HotelZipCode { get; set; } // Nullable

    //    [XmlElement(ElementName = "hotelCountry")]
    //    public string? HotelCountry { get; set; } // Nullable

    //    [XmlElement(ElementName = "hotelCity")]
    //    public string? HotelCity { get; set; } // Nullable
    //}

    //public class Description1
    //{
    //    [XmlElement(ElementName = "language")]
    //    public Language Language { get; set; } = new Language(); // Default to new Language object
    //}

    //public class Language
    //{
    //    [XmlAttribute(AttributeName = "id")]
    //    public string? Id { get; set; } // Nullable

    //    [XmlAttribute(AttributeName = "name")]
    //    public string? Name { get; set; } // Nullable

    //    [XmlText]
    //    public string? Text { get; set; } // Nullable
    //}

    //public class Amenitie
    //{
    //    [XmlAttribute(AttributeName = "count")]
    //    public int Count { get; set; }

    //    [XmlElement(ElementName = "language")]
    //    public Language Language { get; set; } = new Language(); // Default to new Language object
    //}

    //public class Leisure
    //{
    //    [XmlAttribute(AttributeName = "count")]
    //    public int Count { get; set; }

    //    [XmlElement(ElementName = "language")]
    //    public Language Language { get; set; } = new Language(); // Default to new Language object
    //}

    //public class Business
    //{
    //    [XmlAttribute(AttributeName = "count")]
    //    public int Count { get; set; }

    //    [XmlElement(ElementName = "language")]
    //    public Language Language { get; set; } = new Language(); // Default to new Language object
    //}

    //public class Transportation
    //{
    //    [XmlElement(ElementName = "airports")]
    //    public Airports Airports { get; set; } = new Airports(); // Default to new Airports object

    //    [XmlElement(ElementName = "rails")]
    //    public Rails Rails { get; set; } = new Rails(); // Default to new Rails object

    //    [XmlElement(ElementName = "subways")]
    //    public Subways Subways { get; set; } = new Subways(); // Default to new Subways object

    //    [XmlElement(ElementName = "cruises")]
    //    public Cruises Cruises { get; set; } = new Cruises(); // Default to new Cruises object
    //}

    //public class Airports
    //{
    //    [XmlElement(ElementName = "airport")]
    //    public Airport Airport { get; set; } = new Airport(); // Default to new Airport object
    //}

    //public class Airport
    //{
    //    [XmlElement(ElementName = "name")]
    //    public string? Name { get; set; } // Nullable

    //    [XmlElement(ElementName = "dist")]
    //    public List<Distance> Distance { get; set; } = new List<Distance>(); // Initialize as empty list

    //    [XmlElement(ElementName = "directions")]
    //    public string? Directions { get; set; } // Nullable
    //}

    //public class Distance
    //{
    //    [XmlAttribute(AttributeName = "attr")]
    //    public string? Attr { get; set; } // Nullable

    //    [XmlText]
    //    public string? Value { get; set; } // Nullable
    //}

    //public class Rails
    //{
    //    [XmlElement(ElementName = "rail")]
    //    public Rail Rail { get; set; } = new Rail(); // Default to new Rail object
    //}

    //public class Rail
    //{
    //    [XmlElement(ElementName = "name")]
    //    public string? Name { get; set; } // Nullable

    //    [XmlElement(ElementName = "dist")]
    //    public List<Distance> Distance { get; set; } = new List<Distance>(); // Initialize as empty list

    //    [XmlElement(ElementName = "directions")]
    //    public string? Directions { get; set; } // Nullable
    //}

    //public class Subways
    //{
    //    [XmlElement(ElementName = "subway")]
    //    public Subway Subway { get; set; } = new Subway(); // Default to new Subway object
    //}

    //public class Subway
    //{
    //    [XmlElement(ElementName = "name")]
    //    public string? Name { get; set; } // Nullable

    //    [XmlElement(ElementName = "dist")]
    //    public List<Distance> Distance { get; set; } = new List<Distance>(); // Initialize as empty list

    //    [XmlElement(ElementName = "directions")]
    //    public string? Directions { get; set; } // Nullable
    //}

    //public class Cruises
    //{
    //    [XmlElement(ElementName = "cruise")]
    //    public Cruise Cruise { get; set; } = new Cruise(); // Default to new Cruise object
    //}

    //public class Cruise
    //{
    //    [XmlElement(ElementName = "name")]
    //    public string? Name { get; set; } // Nullable

    //    [XmlElement(ElementName = "dist")]
    //    public List<Distance> Distance { get; set; } = new List<Distance>(); // Initialize as empty list

    //    [XmlElement(ElementName = "directions")]
    //    public string? Directions { get; set; } // Nullable
    //}

    //public class GeoPoint
    //{
    //    [XmlElement(ElementName = "latitude")]
    //    public double Latitude { get; set; }

    //    [XmlElement(ElementName = "longitude")]
    //    public double Longitude { get; set; }
    //}

    //public class Rooms
    //{
    //    [XmlAttribute(AttributeName = "count")]
    //    public int Count { get; set; }

    //    [XmlElement(ElementName = "room")]
    //    public List<Room> Room { get; set; } = new List<Room>(); // Initialize as empty list
    //}

    //public class Room
    //{
    //    [XmlElement(ElementName = "roomType")]
    //    public string? RoomType { get; set; } // Nullable

    //    [XmlElement(ElementName = "roomDescription")]
    //    public string? RoomDescription { get; set; } // Nullable

    //    [XmlElement(ElementName = "roomCategory")]
    //    public string? RoomCategory { get; set; } // Nullable

    //    [XmlElement(ElementName = "roomRate")]
    //    public double RoomRate { get; set; }

    //    [XmlElement(ElementName = "roomAmenities")]
    //    public List<string?> RoomAmenities { get; set; } = new List<string?>(); // Nullable list
    //}
    // using System.Xml.Serialization;
    // XmlSerializer serializer = new XmlSerializer(typeof(Result));
    // using (StringReader reader = new StringReader(xml))
    // {
    //    var test = (Result)serializer.Deserialize(reader);
    // }

}
