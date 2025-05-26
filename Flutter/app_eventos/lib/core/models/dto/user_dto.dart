class UserDto {
  final int? userId;
  final int? name;
  final int? lastName;
  final int? phone;
  final String? birthDate;
  final int? city;
  final int? country;
  final String username;
  final String email;
  final String password;
  final String? role;

  UserDto({
    this.userId,
    this.name,
    this.lastName,
    this.phone,
    this.birthDate,
    this.city,
    this.country,
    required this.username,
    required this.email,
    required this.password,
    this.role,
  });

  factory UserDto.fromJson(Map<String, dynamic> json) => UserDto(
    userId: json['userId'],
    name: json['name'],
    lastName: json['lastName'],
    phone: json['phone'],
    birthDate: json['birthDate'],
    city: json['city'],
    country: json['country'],
    username: json['username'],
    email: json['email'],
    password: json['password'],
    role: json['role'],
  );

  Map<String, dynamic> toJson() => {
    'userId': userId,
    'name': name,
    'lastName': lastName,
    'phone': phone,
    'birthDate': birthDate,
    'city': city,
    'country': country,
    'username': username,
    'email': email,
    'password': password,
    'role': role,
  };
}