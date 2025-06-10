import 'package:app_eventos/features/auth/widgets/app_text_field.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:flutter_signin_button/flutter_signin_button.dart';

import '../../../core/auth/auth_provider.dart';
import 'register_screen.dart';
import 'package:app_eventos/features/auth/widgets/app_button.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({Key? key}) : super(key: key);

  @override
  _LoginScreenState createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final _formKey = GlobalKey<FormState>();
  final _usernameController = TextEditingController();
  final _passwordController = TextEditingController();
  bool _isLoading = false;

  @override
  void dispose() {
    _usernameController.dispose();
    _passwordController.dispose();
    super.dispose();
  }

  Future<void> _submit() async {
    if (!_formKey.currentState!.validate()) return;

    setState(() => _isLoading = true);

    try {
      final authProvider = Provider.of<AuthProvider>(context, listen: false);
      final user = await authProvider.loginWithEmail(
        '',
        _passwordController.text.trim(),
        _usernameController.text.trim(),
      );

      if (user != null && mounted) {
        Navigator.pushReplacementNamed(context, '/main');
      } else if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Usuario o contraseña incorrectos')),
        );
      }
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(
          context,
        ).showSnackBar(SnackBar(content: Text('Error: ${e.toString()}')));
      }
    } finally {
      if (mounted) setState(() => _isLoading = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.transparent,
      body: Stack(
        children: [
          Container(
            decoration: const BoxDecoration(
              gradient: LinearGradient(
                colors: [Color(0xFFB993D6), Color(0xFF8CA6DB)],
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
              ),
            ),
          ),
          SingleChildScrollView(
            padding: const EdgeInsets.all(20),
            child: Column(
              children: [
                const SizedBox(height: 60),
                Text(
                  'Inicia Sesión',
                  style: Theme.of(context).textTheme.headlineMedium?.copyWith(
                    color: Colors.deepPurple,
                    fontWeight: FontWeight.bold,
                  ),
                ),
                const SizedBox(height: 30),
                Form(
                  key: _formKey,
                  child: Column(
                    children: [
                      AppTextField(
                        controller: _usernameController,
                        label: 'Usuario',
                        icon: Icons.person,
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Por favor ingresa tu usuario';
                          }
                          return null;
                        },
                      ),
                      const SizedBox(height: 20),
                      TextFormField(
                        controller: _passwordController,
                        decoration: const InputDecoration(
                          labelText: 'Contraseña',
                          floatingLabelBehavior: FloatingLabelBehavior.never,

                          prefixIcon: Icon(Icons.lock),
                        ),
                        obscureText: true,
                        validator: (value) {
                          if (value == null || value.isEmpty) {
                            return 'Por favor ingresa tu contraseña';
                          }
                          if (value.length < 6) {
                            return 'La contraseña debe tener al menos 6 caracteres';
                          }
                          return null;
                        },
                      ),
                      const SizedBox(height: 30),
                      SizedBox(
                        width: double.infinity,
                        child: AppButton(
                          label: 'Iniciar Sesión',
                          icon: Icons.login,
                          loading: _isLoading,
                          onPressed: _submit,
                        ),
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: 20),
                TextButton(
                  onPressed: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (context) => const RegisterScreen(),
                      ),
                    );
                  },
                  child: const Text('¿No tienes cuenta? Regístrate'),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
