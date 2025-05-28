class RegisterDto {
  final String name;
  final String lastName;
  final int? phone;
  final String? birthDate; // yyyy-MM-dd
  final String? city;
  final String? country;
  final String username;
  final String email;
  final String password;
  final String? role;

  RegisterDto({
    required this.name,
    required this.lastName,
    required this.phone,
    required this.birthDate,
    required this.city,
    required this.country,
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

  factory RegisterDto.fromJson(Map<String, dynamic> json) {
    return RegisterDto(
      name: json['name'],
      lastName: json['lastName'],
      phone: json['phone'] != null ? int.tryParse(json['phone'].toString()) : null,
      birthDate: json['birthDate'],
      city: json['city'],
      country: json['country'],
      username: json['username'],
      email: json['email'],
      password: json['password'],
      role: json['role'],
    );
  }
}