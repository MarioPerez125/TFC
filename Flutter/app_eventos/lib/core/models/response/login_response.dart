import '../dto/auth_dto.dart';
import '../dto/user_dto.dart';

class LoginResponse {
  final AuthDto authDto;
  final UserDto user;
  final String token;
  final DateTime tokenExpiration;
  final bool isSuccess;
  final String? message;
  final int? responseCode;

  LoginResponse({
    required this.authDto,
    required this.user,
    required this.token,
    required this.tokenExpiration,
    required this.isSuccess,
    this.message,
    this.responseCode,
  });

  factory LoginResponse.fromJson(Map<String, dynamic> json) => LoginResponse(
    authDto: AuthDto.fromJson(json['authDto']),
    user: UserDto.fromJson(json['user']),
    token: json['token'],
    tokenExpiration: DateTime.parse(json['tokenExpiration']),
    isSuccess: json['isSuccess'],
    message: json['message'],
    responseCode: json['responseCode'],
  );
  Map<String, dynamic> toJson() => {
    'authDto': authDto.toJson(),
    'user': user.toJson(),
    'token': token,
    'tokenExpiration': tokenExpiration.toIso8601String(),
    'isSuccess': isSuccess,
    'message': message,
    'responseCode': responseCode,
  };
}
