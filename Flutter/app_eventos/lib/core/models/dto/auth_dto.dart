class AuthDto {
  final int? name;
  final int? lastName;
  final int? phone;
  final String? birthDate; // Usamos String para simplificar el manejo de fechas
  final int? city;
  final int? country;
  final String username;
  final String email;
  final String password;
  final String? role;

  AuthDto({
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

  Map<String, dynamic> toJson() => {
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

  factory AuthDto.fromJson(Map<String, dynamic> json) => AuthDto(
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
}