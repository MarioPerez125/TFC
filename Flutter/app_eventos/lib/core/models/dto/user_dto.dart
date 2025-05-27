class UserDto {
  final int? userId;
  final String? name;
  final String? lastName;
  final int phone;
  final String? birthDate;
  final String? city;
  final String? country;
  final String? username;
  final String? email;
  final String? password;
  final String? role;

  UserDto({
    this.userId,
    required this.name,
    required this.lastName,
    required this.phone,
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