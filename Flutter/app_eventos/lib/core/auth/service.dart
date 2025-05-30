import 'dart:convert';
import 'package:app_eventos/core/models/dto/fighter_dto.dart';
import 'package:app_eventos/core/models/dto/fighter_for_friend_dto.dart';
import 'package:app_eventos/core/models/dto/register_dto.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:shared_preferences/shared_preferences.dart';
import '../api/api_client.dart';
import '../api/endpoints.dart';
import '../models/dto/auth_dto.dart';
import '../models/response/login_response.dart';
import '../models/dto/user_dto.dart';

class Service {
  final FlutterSecureStorage _secureStorage = const FlutterSecureStorage();
  final ApiClient _apiClient = ApiClient();

  Future<UserDto?> loginWithEmail(
    String email,
    String password,
    String username,
  ) async {
    print('POST a: ${Endpoints.login}');
    print('Body: {"username": $username, "password": $password}');
    final response = await _apiClient.post(
      Endpoints.login,
      body: {'username': username, 'password': password},
      requiresAuth: false,
    );
    print(
      'Respuesta: ${response.statusCode} ${response.data} ${response.error}',
    );
    if (response.data != null && response.statusCode == 200) {
      final loginResponse = LoginResponse.fromJson(response.data);
      await _saveAuthData(loginResponse);
      return loginResponse.user;
    }
    return null;
  }

  Future<void> _saveAuthData(LoginResponse loginResponse) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setString('auth_data', jsonEncode(loginResponse.toJson()));
    print('Guardando token: ${loginResponse.token}');
    await _secureStorage.write(key: 'jwt_token', value: loginResponse.token);
    print('Token guardado');
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

  Future<RegisterDto?> register({
    required String name,
    required String lastName,
    required String email,
    required String password,
    required String username,
    int? phone,
    String? birthDate,
    String? city,
    String? country,
  }) async {
    final registerDto = RegisterDto(
      name: name,
      lastName: lastName,
      email: email,
      password: password,
      username: username,
      phone: phone,
      birthDate: birthDate,
      city: city,
      country: country,
    );
    print('Body enviado: ${registerDto.toJson()}');
    final response = await _apiClient.post(
      Endpoints.register,
      body: registerDto.toJson(), // <-- Aquí va el mapa directamente
      requiresAuth: false,
    );
    print('ruta: ${Endpoints.register}');
    print('Status: ${response.statusCode}');
    print('Respuesta backend: ${response.data}');
    if (response.statusCode == 200 || response.statusCode == 201) {
      return RegisterDto.fromJson(response.data);
    }
    return null;
  }

  Future<void> logout() async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.remove('auth_data');
    await _secureStorage.delete(key: 'jwt_token');
  }

  Future<bool> isLoggedIn() async {
    final token = await _secureStorage.read(key: 'jwt_token');
    return token != null;
  }

  Future<UserDto?> registerAsOrganizer(AuthDto authDto) async {
    final response = await _apiClient.post(
      Endpoints.registerAsOrganizer,
      body: authDto.toJson(),
      requiresAuth: false,
    );
    if (response.statusCode == 200 && response.data != null) {
      return UserDto.fromJson(response.data);
    }
    return null;
  }

  Future<UserDto?> registerAsFighter(FighterDto fighterDto) async {
    final response = await _apiClient.post(
      Endpoints.registerAsFighter,
      body: fighterDto.toJson(),
      requiresAuth: false,
    );
    
    if (response.statusCode == 200 && response.data != null) {
      return UserDto.fromJson(response.data);
    }
    return null;
  }

  Future<void> saveUserDto(UserDto userDto) async {
    final prefs = await SharedPreferences.getInstance();
    final authData = prefs.getString('auth_data');
    if (authData != null) {
      final Map<String, dynamic> decoded = jsonDecode(authData);
      decoded['user'] = userDto.toJson();
      await prefs.setString('auth_data', jsonEncode(decoded));
    }
  }
  
  Future<FighterDto?> getFighterInfo(int userId) async {
    final response = await _apiClient.get(
      '${Endpoints.fighterInfo}/$userId',
      requiresAuth: false,
    );
    print('${Endpoints.fighterInfo}/$userId');
    if (response.statusCode == 200 && response.data != null) {
      return FighterDto.fromJson(response.data);
    }
    return null;
  }

  Future<List<FighterForFriendDto>> getAllFighters() async {
    final response = await _apiClient.get(
      Endpoints.getFighters, 
      requiresAuth: false
    );
    print('Respuesta fighters: ${response.data}');
    if (response.statusCode == 200 && response.data is List) {
      return (response.data as List)
          .map((e) => FighterForFriendDto.fromJson(e as Map<String, dynamic>))
          .toList();
    }
    if (response.data is Map && response.data['fighterList'] is List) {
      return (response.data['fighterList'] as List)
          .map((e) => FighterForFriendDto.fromJson(e as Map<String, dynamic>))
          .toList();
    }
    return [];
  }
}