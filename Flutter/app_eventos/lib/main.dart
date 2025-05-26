import 'package:app_eventos/core/auth/auth_service.dart';
import 'package:app_eventos/features/auth/screens/login_screen.dart';
import 'package:app_eventos/features/auth/screens/main_screen.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

import '../../core/auth/auth_provider.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  
  // Verificar el estado de autenticación antes de iniciar la app
  final storage = FlutterSecureStorage();
  final token = await storage.read(key: 'jwt_token');
  final isLoggedIn = token != null;

  runApp(
    MultiProvider(
      providers: [
        ChangeNotifierProvider(
          create: (_) => AuthProvider(AuthService()),
        ),
      ],
      child: MyApp(isLoggedIn: isLoggedIn),
    ),
  );
}

class MyApp extends StatelessWidget {
  final bool isLoggedIn;

  const MyApp({super.key, required this.isLoggedIn});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Fans de Deportes de Contacto',
      // Mostrar MainScreen si está logueado, LoginScreen si no
      home: isLoggedIn ? const MainScreen() : const LoginScreen(),
      routes: {
        '/login': (context) => const LoginScreen(),
        '/main': (context) => const MainScreen(),
      },
    );
  }
}