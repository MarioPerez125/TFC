import 'dart:convert';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:shared_preferences/shared_preferences.dart';
import '../api/api_client.dart';
import '../api/endpoints.dart';
import '../models/dto/auth_dto.dart';
import '../models/response/login_response.dart';
import '../models/dto/user_dto.dart';

class AuthService {
  final FlutterSecureStorage _secureStorage = const FlutterSecureStorage();
  final ApiClient _apiClient = ApiClient();

Future<UserDto?> loginWithEmail(String email, String password, String username) async {
  final authDto = AuthDto(
    name: null,
    lastName: null,
    phone: null,
    birthDate: null,
    city: null,
    country: null,
    username: username,
    email: email,
    password: password,
    role: null,
  );
  final response = await _apiClient.post(
    Endpoints.login,
    body: {'authDto': authDto.toJson()},
  );

  if (response.data != null) {
    final loginResponse = LoginResponse.fromJson(response.data);
    await _saveAuthData(loginResponse);
    return loginResponse.user;
  }
  return null;
}

  Future<void> _saveAuthData(LoginResponse loginResponse) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setString('auth_data', jsonEncode(loginResponse.toJson()));
    await _secureStorage.write(key: 'jwt_token', value: loginResponse.token);
  }

  Future<String?> getToken() async {
    return await _secureStorage.read(key: 'jwt_token');
  }

  Future<UserDto?> getCurrentUser() async {
    final prefs = await SharedPreferences.getInstance();
    final authData = prefs.getString('auth_data');
    if (authData == null) return null;
    final Map<String, dynamic> decoded = jsonDecode(authData);
    if (decoded['user'] != null) {
      return UserDto.fromJson(decoded['user']);
    }
    return null;
  }
// ...existing code...
Future<bool> register({
  required String name,
  required String lastName,
  required String email,
  required String password,
  required String username,
}) async {
  final authDto = AuthDto(
    name: int.tryParse(name),
    lastName: int.tryParse(lastName),
    phone: null,
    birthDate: null,
    city: null,
    country: null,
    username: username,
    email: email,
    password: password,
    role: null,
  );
  final response = await _apiClient.post(
    Endpoints.register,
    body: {'authDto': authDto.toJson()},
    requiresAuth: false,
  );
  return response.statusCode == 200 || response.statusCode == 201;
}
// ...existing code...
  Future<void> logout() async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.remove('auth_data');
    await _secureStorage.delete(key: 'jwt_token');
  }

  Future<bool> isLoggedIn() async {
    final token = await _secureStorage.read(key: 'jwt_token');
    return token != null;
  }
}