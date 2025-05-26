import 'package:app_eventos/core/models/dto/user_dto.dart';

class User {
  final int? userId;
  final String name;
  final String lastName;
  final String? phone;
  final DateTime? birthDate;
  final String? city;
  final String? country;
  final String username;
  final String email;
  final String? password;
  final String? role;

  User({
    this.userId,
    required this.name,
    required this.lastName,
    this.phone,
    this.birthDate,
    this.city,
    this.country,
    required this.username,
    required this.email,
    this.password,
    this.role = 'User',
  });

  factory User.fromJson(Map<String, dynamic> json) {
    return User(
      userId: json['userId'],
      name: json['name'],
      lastName: json['lastName'],
      phone: json['phone'],
      birthDate: json['birthDate'] != null ? DateTime.parse(json['birthDate']) : null,
      city: json['city'],
      country: json['country'],
      username: json['username'],
      email: json['email'],
      role: json['role'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'userId': userId,
      'name': name,
      'lastName': lastName,
      'phone': phone,
      'birthDate': birthDate?.toIso8601String(),
      'city': city,
      'country': country,
      'username': username,
      'email': email,
      'password': password,
      'role': role,
    };
  }

  factory User.fromDto(UserDto dto) {
    return User(
      userId: dto.userId,
      name: dto.name?.toString() ?? '',
      lastName: dto.lastName?.toString() ?? '',
      phone: dto.phone?.toString(),
      birthDate: dto.birthDate != null ? DateTime.tryParse(dto.birthDate!) : null,
      city: dto.city?.toString(),
      country: dto.country?.toString(),
      username: dto.username,
      email: dto.email,
      password: dto.password,
      role: dto.role,
    );
  }
}
