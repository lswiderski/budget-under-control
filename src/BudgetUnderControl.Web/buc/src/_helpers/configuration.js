  
import dotenv from 'dotenv'
dotenv.config()

export default class Configuration {
  static get CONFIG () {
    return {
      backendHost: '$VUE_APP_BACKEND_API_URL',
    }
  }

  static value (name) {
    if (!(name in this.CONFIG)) {
      console.log(`Configuration: There is no key named "${name}"`)
      return
    }

    const value = this.CONFIG[name]
    console.log(process.env);
    if (!value) {
      console.log(`Configuration: Value for "${name}" is not defined`)
      return
    }
    console.log(` name: "${name}", value: "${value}"`)
    if (value.startsWith('$VUE_APP_')) {
      // value was not replaced, it seems we are in development.
      // Remove $ and get current value from process.env
      const envName = value.substr(1)
      const envValue = process.env[envName]
      if (envValue) {
        console.log(` name: "${name}", envValue: "${envValue}"`)
        return envValue
      } else {
        console.log(`Configuration: Environment variable "${envName}" is not defined`)
      }
    } else {
      // value was already replaced, it seems we are in production.
      return value
    }
  }
}