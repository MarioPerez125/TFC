class AuthDto {
  final String? username;
  final String? password;

  AuthDto({
    this.username,
    this.password,
  });

  factory AuthDto.fromJson(Map<String, dynamic> json) => AuthDto(
    username: json['username']?.toString(),
    password: json['password']?.toString(),
  );

  Map<String, dynamic> toJson() {
    return {
      'username': username,
      'password': password,
    };
  }
}