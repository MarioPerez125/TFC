class RegisterDto {
  final String name;
  final String lastName;
  final int? phone;
  final String? birthDate;
  final String? city;
  final String? country;
  final String username;
  final String email;
  final String password;
  final String? role;

  RegisterDto({
    required this.name,
    required this.lastName,
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
    'birthDate': birthDate, // <-- debe ser yyyy-MM-dd
    'city': city,
    'country': country,
    'username': username,
    'email': email,
    'password': password,
    'role': role,
  };
}