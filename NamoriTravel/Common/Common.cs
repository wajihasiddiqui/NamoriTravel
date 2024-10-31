using Azure;
using ModelsDTO;
using NamoriTravel.Models;
using Newtonsoft.Json;
using System.Dynamic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NamoriTravel.Common
{
    public static class Common
    {
        public static string GenerateXmlRequest(string xmlTemplate, Dictionary<string, string> parameters)
        {
            foreach (var param in parameters)
            {
                xmlTemplate = xmlTemplate.Replace($"@{param.Key}", param.Value);
            }
            return xmlTemplate;
        }
        public static string FormatDate(string dateInput)
        {
            // Split and trim the input date string
            string[] dateParts = dateInput.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            // Month mapping
            Dictionary<string, string> monthMapping = new Dictionary<string, string>
        {
            { "jan", "01" },
            { "feb", "02" },
            { "mar", "03" },
            { "apr", "04" },
            { "may", "05" },
            { "jun", "06" },
            { "jul", "07" },
            { "aug", "08" },
            { "sep", "09" },
            { "oct", "10" },
            { "nov", "11" },
            { "dec", "12" }
        };

            // Extract day and month
            string day = dateParts[1].Trim().PadLeft(2, '0');
            string month = monthMapping[dateParts[0].Trim().ToLower()];
            string year = dateParts[2].ToString(); // Current year or specify as needed

            return $"{year}-{month}-{day}";
        }


    }
    //.Where(img => !string.IsNullOrEmpty(img.Url) && img.Url.Contains("www.dotwconnect.com"))
    //    .ToList();
    public static class XmlResponseTo_Obj
    {
        //public static HotelResponse XmlResponseToObj1(string responseContent)
        //{
        //    XDocument xmlDoc = XDocument.Parse(responseContent);

        //    var result = new HotelResponse
        //    {
        //        Command = xmlDoc.Root.Attribute("command")?.Value,
        //        TID = xmlDoc.Root.Attribute("tID")?.Value,
        //        Ip = xmlDoc.Root.Attribute("ip")?.Value,
        //        Date = DateTime.Parse(xmlDoc.Root.Attribute("date")?.Value),
        //        Version = xmlDoc.Root.Attribute("version")?.Value,
        //        ElapsedTime = double.Parse(xmlDoc.Root.Attribute("elapsedTime")?.Value),
        //        CurrencyShort = xmlDoc.Root.Element("currencyShort")?.Value,
        //        Hotels = xmlDoc.Descendants("hotel").Select(hotel => new Hotel
        //        {
        //            Runno = hotel.Attribute("runno")?.Value,
        //            Preferred = hotel.Attribute("preferred")?.Value,
        //            Exclusive = hotel.Attribute("exclusive")?.Value,
        //            CityName = hotel.Element("cityName")?.Value,
        //            HotelId = hotel.Attribute("hotelid")?.Value,
        //            HotelName = hotel.Element("hotelName")?.Value,
        //            FullAddress = hotel.Element("fullAddress") != null ? new FullAddress
        //            {
        //                HotelStreetAddress = hotel.Element("fullAddress").Element("hotelStreetAddress")?.Value,
        //                HotelZipCode = hotel.Element("fullAddress").Element("hotelZipCode")?.Value,
        //                HotelCountry = hotel.Element("fullAddress").Element("hotelCountry")?.Value,
        //                HotelCity = hotel.Element("fullAddress").Element("hotelCity")?.Value
        //            } : null,
        //            Amenities = hotel.Descendants("amenitieItem").Select(amenity => new Amenity
        //            {
        //                Id = amenity.Attribute("id")?.Value,
        //                Item = amenity.Value
        //            }).ToList(),
        //            HotelImage = hotel.Descendants("hotelImages").Descendants("image").Select(imageElement => new HotelImages
        //            {
        //                category = imageElement.Element("category")?.Value,
        //                url = imageElement.Element("url")?.Value
        //            }).FirstOrDefault(image => !string.IsNullOrEmpty(image.url)),

        //            Rooms = hotel.Descendants("roomType").Select(roomType => new Room
        //            {
        //                Runno = roomType.Attribute("runno")?.Value,
        //                RoomTypeCode = roomType.Attribute("roomtypecode")?.Value,
        //                Name = roomType.Element("name")?.Value,
        //                RoomInfo = new RoomInfo
        //                {
        //                    MaxAdult = int.Parse(roomType.Element("roomInfo")?.Element("maxAdult")?.Value ?? "0"),
        //                    MaxExtraBed = int.Parse(roomType.Element("roomInfo")?.Element("maxExtraBed")?.Value ?? "0"),
        //                    MaxChildren = int.Parse(roomType.Element("roomInfo")?.Element("maxChildren")?.Value ?? "0")
        //                },
        //                RoomCapacityInfo = new RoomCapacityInfo
        //                {
        //                    RoomPaxCapacity = int.Parse(roomType.Element("roomCapacityInfo")?.Element("roomPaxCapacity")?.Value ?? "0"),
        //                    AllowedAdultsWithoutChildren = int.Parse(roomType.Element("roomCapacityInfo")?.Element("allowedAdultsWithoutChildren")?.Value ?? "0"),
        //                    AllowedAdultsWithChildren = int.Parse(roomType.Element("roomCapacityInfo")?.Element("allowedAdultsWithChildren")?.Value ?? "0"),
        //                    MaxExtraBed = int.Parse(roomType.Element("roomCapacityInfo")?.Element("maxExtraBed")?.Value ?? "0")
        //                }
        //            }).ToList()
        //        }).ToList()
        //    };
        //    return result;
        //}
        public static HotelResponse XmlResponseToObj(string responseContent)
        {
            XDocument xmlDoc = XDocument.Parse(responseContent);

            var result = new HotelResponse
            {
                Command = xmlDoc.Root.Attribute("command")?.Value,
                TID = xmlDoc.Root.Attribute("tID")?.Value,
                Ip = xmlDoc.Root.Attribute("ip")?.Value,
                Date = DateTime.Parse(xmlDoc.Root.Attribute("date")?.Value),
                Version = xmlDoc.Root.Attribute("version")?.Value,
                ElapsedTime = double.Parse(xmlDoc.Root.Attribute("elapsedTime")?.Value),
                CurrencyShort = xmlDoc.Root.Element("currencyShort")?.Value,
                Count = int.TryParse(xmlDoc.Root.Element("hotels")?.Attribute("count")?.Value, out var count) ? count : 0,
                Hotels = xmlDoc.Descendants("hotel").Select(hotel => new Hotel
                {
                    Count = int.TryParse(hotel?.Attribute("count")?.Value, out var count) ? count : 0,
                    Runno = hotel.Attribute("runno")?.Value,
                    Preferred = hotel.Attribute("preferred")?.Value,
                    Exclusive = hotel.Attribute("exclusive")?.Value,
                    HotelId = hotel.Attribute("hotelid")?.Value,
                    CityName = hotel.Element("cityName")?.Value,
                    HotelName = hotel.Element("hotelName")?.Value,
                    Location = hotel.Element("location")?.Value,
                    StateName = hotel.Element("stateName")?.Value,
                    CountryName = hotel.Element("countryName")?.Value,
                    RegionName = hotel.Element("regionName")?.Value,
                    Description1 = hotel.Element("description1")?.Value.ToString(),
                    FullAddress = new FullAddress
                    {
                        HotelStreetAddress = hotel.Element("fullAddress")?.Element("hotelStreetAddress")?.Value,
                        HotelZipCode = hotel.Element("fullAddress")?.Element("hotelZipCode")?.Value,
                        HotelCountry = hotel.Element("fullAddress")?.Element("hotelCountry")?.Value,
                        HotelCity = hotel.Element("fullAddress")?.Element("hotelCity")?.Value
                    }, 
                    geoPoint = new GeoPoint
                    {
                        Lat = hotel.Element("geoPoint")?.Element("lat")?.Value,
                        lng = hotel.Element("geoPoint")?.Element("lng")?.Value,
                    },
                    Amenities = hotel.Descendants("amenitieItem").Select(a => new Amenity
                    {
                        Id = a.Attribute("id")?.Value,
                        Item = a.Value
                    }).ToList(),
                    Businesses = hotel.Descendants("businessItem").Select(a => new Business
                    {
                        Id = a.Attribute("id")?.Value,
                        businessItem = a.Value
                    }).ToList(),
                    
                    HotelImages = hotel.Descendants("image")
                    .Where(image => image.Element("url")?.Value.Contains("dotwconnect") == true).
                    Select(image => new HotelImages
                    {
                        Category = image.Element("category")?.Value,
                        Url = image.Element("url")?.Value
                    }).ToList(),
                    Rooms = hotel.Descendants("roomType").Select(r => new Room
                    {
                        Runno = r.Attribute("runno")?.Value,
                        RoomTypeCode = r.Attribute("roomtypecode")?.Value,
                        Name = r.Element("name")?.Value,
                        RoomInfo = new RoomInfo
                        {
                            MaxAdult = int.Parse(r.Element("roomInfo")?.Element("maxAdult")?.Value ?? "0"),
                            MaxExtraBed = int.Parse(r.Element("roomInfo")?.Element("maxExtraBed")?.Value ?? "0"),
                            MaxChildren = int.Parse(r.Element("roomInfo")?.Element("maxChildren")?.Value ?? "0")
                        },
                        RoomCapacityInfo = new RoomCapacityInfo
                        {
                            RoomPaxCapacity = int.Parse(r.Element("roomCapacityInfo")?.Element("roomPaxCapacity")?.Value ?? "0"),
                            AllowedAdultsWithChildren = int.Parse(r.Element("roomCapacityInfo")?.Element("allowedAdultsWithChildren")?.Value ?? "0"),
                            AllowedAdultsWithoutChildren = int.Parse(r.Element("roomCapacityInfo")?.Element("allowedAdultsWithoutChildren")?.Value ?? "0")
                        }
                    }).ToList()
                }).ToList()
            };
            return result;
        }


    }
    public static class XmlHelper
    {
        public static XDocument RemoveNamespace(XDocument document)
        {
            foreach (var element in document.Descendants())
            {
                var newName = XName.Get(element.Name.LocalName);
                element.Name = newName;

                var newAttributes = element.Attributes().Select(attr =>
                    new XAttribute(XName.Get(attr.Name.LocalName), attr.Value)).ToArray();

                foreach (var attr in element.Attributes().ToList())
                {
                    attr.Remove();
                }

                foreach (var attr in newAttributes)
                {
                    element.Add(attr);
                }
            }

            return document;
        }
        public static T DeserializeXmlToObject<T>(string xmlString)
        {
            if (string.IsNullOrEmpty(xmlString))
            {
                throw new ArgumentNullException(nameof(xmlString), "The XML string cannot be null or empty.");
            }

            try
            {
                var document = XDocument.Parse(xmlString);
                var documentWithoutNamespace = RemoveNamespace(document);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                //using (var reader = documentWithoutNamespace.CreateReader())
                //{
                //    return (T)serializer.Deserialize(reader);
                //}
                using (StringReader stringReader = new StringReader(xmlString))
                {
                    return (T)serializer.Deserialize(stringReader);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while deserializing the XML string to object: {ex.Message}", ex);
            }
        }
        public static T ConvertXmlToJson<T>(string xmlString)
        {
            var document = XDocument.Parse(xmlString);
            try
            {
                var documentWithoutNamespace = RemoveNamespace(document);
            var serializer = new XmlSerializer(typeof(T));
                using (StringReader stringReader = new StringReader(xmlString))
                {
                    return (T)serializer.Deserialize(stringReader);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while deserializing the XML string to object: {ex.Message}", ex);
            }
        }
    }

    public static class XmlDynamicParser
    {
        public static dynamic ParseXmlToDynamic(string xmlString)
        {
            var element = XElement.Parse(xmlString);
            return ParseElement(element);
        }
        private static dynamic ParseElement(XElement element)
        {
            dynamic obj = new ExpandoObject();
            var dict = (IDictionary<string, object>)obj;

            foreach (var attribute in element.Attributes())
            {
                dict[attribute.Name.LocalName] = attribute.Value;
            }

            foreach (var child in element.Elements())
            {
                if (dict.ContainsKey(child.Name.LocalName))
                {
                    // Handle duplicate keys (e.g., multiple children with the same name)
                    if (dict[child.Name.LocalName] is List<dynamic> list)
                    {
                        list.Add(ParseElement(child));
                    }
                    else
                    {
                        list = new List<dynamic> { dict[child.Name.LocalName], ParseElement(child) };
                        dict[child.Name.LocalName] = list;
                    }
                }
                else
                {
                    dict[child.Name.LocalName] = ParseElement(child);
                }
            }

            return obj;
        }
        public static string ConvertXmlToJson(string xmlString)
        {
            dynamic dynamicObject = ParseXmlToDynamic(xmlString);
            return JsonConvert.SerializeObject(dynamicObject);
        }
        public static HotelResponse MapToHotelResponse(dynamic parsedData)
        {
            return new HotelResponse
            {
                Command = Convert.ToString(parsedData.command),
                TID = Convert.ToString(parsedData.tID),
                Ip = Convert.ToString(parsedData.ip),
                Date = DateTime.TryParse(Convert.ToString(parsedData.date), out DateTime dt) ?  dt : DateTime.MinValue,
                Version = Convert.ToString(parsedData.version),
                ElapsedTime = double.TryParse(Convert.ToString(parsedData.elapsedTime), out double elapsedTime) ? elapsedTime : 0,
                Hotels = ((IEnumerable<dynamic>)parsedData.hotels.hotel).Select(MapToHotel).ToList()
            };
        }
        private static Hotel MapToHotel(dynamic hotelData)
        {
            return new Hotel
            {
                HotelId = hotelData.hotelid,
                HotelName = hotelData.hotelName,
                CityName = hotelData.cityName,
                FullAddress = hotelData.fullAddress != null ? new FullAddress
                {
                    HotelStreetAddress = hotelData.fullAddress.hotelStreetAddress,
                    HotelZipCode = hotelData.fullAddress.hotelZipCode,
                    HotelCountry = hotelData.fullAddress.hotelCountry,
                    HotelCity = hotelData.fullAddress.hotelCity
                } : null,
                Amenities = hotelData.amenitieItem != null
                    ? ((IEnumerable<dynamic>)hotelData.amenitieItem).Select(MapToAmenity).ToList()
                    : new List<Amenity>(),
                Rooms = hotelData.roomType != null
                    ? ((IEnumerable<dynamic>)hotelData.roomType).Select(MapToRoom).ToList()
                    : new List<Room>()
            };
        }
        private static Amenity MapToAmenity(dynamic amenityData)
        {
            return new Amenity
            {
                Id = amenityData.id,
                Item = amenityData.Value
            };
        }
        private static Room MapToRoom(dynamic roomData)
        {
            return new Room
            {
                Runno = roomData.runno,
                RoomTypeCode = roomData.roomtypecode,
                Name = roomData.name,
                RoomInfo = roomData.roomInfo != null ? new RoomInfo
                {
                    MaxAdult = int.Parse(roomData.roomInfo.maxAdult.ToString()),
                    MaxExtraBed = int.Parse(roomData.roomInfo.maxExtraBed.ToString()),
                    MaxChildren = int.Parse(roomData.roomInfo.maxChildren.ToString())
                } : null
            };
        }
    }
}
