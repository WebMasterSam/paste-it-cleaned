import React from "react"

import "./PluginConfig.less"
import { FormControlLabel, Switch } from "@material-ui/core"

export interface PluginConfigProps {}

class PluginConfig extends React.Component<PluginConfigProps> {
  render() {
    return (
      <div>
        Plugin Config
        <FormControlLabel
          control={
            <Switch
              checked={true}
              //onChange={handleChange('checkedB')}
              value="checkedB"
              color="primary"
            />
          }
          label="Primary"
        />
        <pre>
          {JSON.stringify({
            Business: {
              Contact: {
                Address: "-",
                City: "-",
                Country: "-",
                FirstName: "-",
                LastName: "-",
                PhoneNumber: "-",
                State: "-"
              },
              Name: "Weblex"
            },
            Configs: [
              {
                Common: {
                  EmbedExternalImages: true,
                  RemoveEmptyTags: true,
                  RemoveSpanTags: true
                },
                Name: "DEFAULT",
                Office: {},
                Web: {
                  RemoveClassNames: true,
                  RemoveIframes: true,
                  RemoveTagAttributes: true
                }
              }
            ],
            Contact: {
              Address: "-",
              City: "-",
              Country: "-",
              FirstName: "-",
              LastName: "-",
              PhoneNumber: "-",
              State: "-"
            }
          })}
        </pre>
      </div>
    )
  }
}

export default PluginConfig
