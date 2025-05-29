import 'package:app_eventos/core/models/dto/auth_dto.dart';
import 'package:app_eventos/core/models/dto/register_dto.dart';
import 'package:app_eventos/core/models/dto/user_dto.dart';
import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import '../models/user_model.dart';
import 'auth_service.dart';

class AuthProvider with ChangeNotifier {
  final AuthService _authService;
  User? _user;
  bool _isLoading = false;

  AuthProvider(this._authService);

  User? get user => _user;
  bool get isLoading => _isLoading;

  Future<void> initialize() async {
    final userDto = await _authService.getCurrentUser();
    _user = userDto != null ? User.fromDto(userDto) : null;
    notifyListeners();
  }

  Future<User?> loginWithEmail(String email, String password, String username) async {
    _isLoading = true;
    notifyListeners();

    try {
      final userDto = await _authService.loginWithEmail(email, password, username);
      _user = userDto != null ? User.fromDto(userDto) : null;
      return _user;
    } finally {
      _isLoading = false;
      notifyListeners();
    }
  }

  Future<RegisterDto?> register(
    String name,
    String lastName,
    String email,
    String password,
    String username,
    int phone,
    String birthDate,
    String city,
    String country,
  ) async {
    _isLoading = true;
    notifyListeners();
    try {
      final registerDto = await _authService.register(
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
      return registerDto;
    } finally {
      _isLoading = false;
      notifyListeners();
    }
  }

  Future<void> logout() async {
    await _authService.logout();
    _user = null;
    notifyListeners();
  }

  Future<void> refreshUser() async {
    final userDto = await _authService.getCurrentUser();
    _user = userDto != null ? User.fromDto(userDto) : null;
    notifyListeners();
  }

  Future<void> registerAsOrganizer(AuthDto authDto, BuildContext context) async {
    final userDto = await AuthService().registerAsOrganizer(authDto);
    if (userDto != null && context.mounted) {
      await setUserFromDto(userDto);
      await _showResultDialog(
        context,
        true,
        '¡Solicitud para ser organizador enviada con éxito!',
      );
    } else if (context.mounted) {
      await _showResultDialog(
        context,
        false,
        'No se pudo enviar la solicitud.',
      );
    }
  }

  Future<void> setUserFromDto(UserDto userDto) async {
    _user = User.fromDto(userDto);
    await _authService.saveUserDto(userDto); // <-- Añade esto
    notifyListeners();
  }

  Future<void> _showResultDialog(BuildContext context, bool success, String message) async {
    // Implementa la lógica para mostrar el diálogo con el resultado
  }
}