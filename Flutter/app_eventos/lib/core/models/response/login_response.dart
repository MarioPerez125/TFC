import '../dto/auth_dto.dart';
import '../dto/user_dto.dart';

class LoginResponse {
  final bool isSuccess;
  final String message;
  final String? responseCode;
  final AuthDto authDto;
  final UserDto user;
  final String token;
  final DateTime tokenExpiration;

  LoginResponse({
    required this.isSuccess,
    required this.message,
    this.responseCode,
    required this.authDto,
    required this.user,
    required this.token,
    required this.tokenExpiration,
  });

  factory LoginResponse.fromJson(Map<String, dynamic> json) => LoginResponse(
    isSuccess: json['isSuccess'],
    message: json['message'],
    responseCode: json['responseCode'],
    authDto: AuthDto.fromJson(json['authDto']),
    user: UserDto.fromJson(json['user']),
    token: json['token'],
    tokenExpiration: DateTime.parse(json['tokenExpiration']),
  );

  Map<String, dynamic> toJson() => {
    'isSuccess': isSuccess,
    'message': message,
    'responseCode': responseCode,
    'authDto': authDto.toJson(),
    'user': user.toJson(),
    'token': token,
    'tokenExpiration': tokenExpiration.toIso8601String(),
  };
}