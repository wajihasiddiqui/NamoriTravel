using ModelsDTO;

namespace NamoriTravel.Models
{
    public class HotelResponse
    {
        public string Command { get; set; }
        public int Count { get; set; }
        public string TID { get; set; }
        public string Ip { get; set; }
        public DateTime Date { get; set; }
        public string Version { get; set; }
        public double ElapsedTime { get; set; }
        public string CurrencyShort { get; set; }
        public List<Hotel> Hotels { get; set; }
        public List<Amenity> Amenities { get; set; }
        public List<AmenitiesDTO> Amenitiesdto { get; set; }
        public List<RateBasisDTO> RateBasis { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public SearchParam searchParam { get; set; }
    }
    public class HotelDetail
    {
        public string Command { get; set; }
        public string TID { get; set; }
        public string Ip { get; set; }
        public DateTime Date { get; set; }
        public string Version { get; set; }
        public double ElapsedTime { get; set; }
        public string CurrencyShort { get; set; }
        public Hotel Hotel { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
    public class Hotel
    {
        public int Count { get; set; }
        public string Runno { get; set; }
        public string Preferred { get; set; }
        public string Exclusive { get; set; }
        public string CityName { get; set; }
        public string HotelId { get; set; }
        public FullAddress FullAddress { get; set; }
        public  GeoPoint geoPoint { get; set; }
        public int LocationId { get; set; }
        public string? StateName { get; set; }
        public string? StateCode { get; set; }
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string? Description1 { get; set; }
        public string? Description2 { get; set; }
        public string? RegionName { get; set; }
        public string? Attraction { get; set; }
        public string? Leisure { get; set; }
        public string? Transportation { get; set; }
        public List<Business> Businesses { get; set; }
        public string HotelName { get; set; }
        public List<Amenity> Amenities { get; set; }
        public List<Amenity> Amenities2 { get; set; }
        public List<AmenitiesDTO> AmenitiesDto { get; set; }
        public List<Room> Rooms { get; set; }
        public List<HotelImages> HotelImages { get; set; }
    }
    public class HotelImages
    {
        public string Category { get; set; }
        public string Url { get; set; }
    }  
    public class GeoPoint
    {
        public string Lat { get; set; }
        public string lng { get; set; }
    } 
 
   
    public class FullAddress
    {
        public string HotelStreetAddress { get; set; }
        public string HotelZipCode { get; set; }
        public string HotelCountry { get; set; }
        public string HotelCity { get; set; }
    }
    public class Amenity
    {
        public string Id { get; set; }
        public string Item { get; set; }
    }
    public class Room
    {
        public string Runno { get; set; }
        public string RoomTypeCode { get; set; }
        public string Name { get; set; }
        public RoomInfo RoomInfo { get; set; }
        public RoomCapacityInfo RoomCapacityInfo { get; set; }
        public List<Amenity> RoomAmenities { get; set; }
    }
    public class Business
    {
        public string Id { get; set; }
        public string businessItem { get; set; }

    }
    public class RoomInfo
    {
        public int MaxAdult { get; set; }
        public int MaxExtraBed { get; set; }
        public int MaxChildren { get; set; }
    }
    public class RoomCapacityInfo
    {
        public int RoomPaxCapacity { get; set; }
        public int AllowedAdultsWithoutChildren { get; set; }
        public int AllowedAdultsWithChildren { get; set; }
        public int MaxExtraBed { get; set; }
    }
}
